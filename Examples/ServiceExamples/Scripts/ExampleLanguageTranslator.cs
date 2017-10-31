﻿/**
* Copyright 2015 IBM Corp. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/

using UnityEngine;
using IBM.Watson.DeveloperCloud.Services.LanguageTranslator.v2;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Connection;
using System.Collections;

public class ExampleLanguageTranslator : MonoBehaviour
{
    private string _pharseToTranslate = "Hello, welcome to IBM Watson!";
    private string _pharseToIdentify = "Hola, donde esta la bibliteca?";
    private string _username = null;
    private string _password = null;
    private string _url = null;

    private LanguageTranslator _languageTranslator;
    private string _baseModelName = "en-es";
    private string _customModelName = "Texan";
    private string _forcedGlossaryFilePath;
    private string _customLanguageModelId;

    private bool _getTranslationTested = false;
    private bool _getModelsTested = false;
    private bool _createModelTested = false;
    private bool _getModelTested = false;
    private bool _deleteModelTested = false;
    private bool _identifyTested = false;
    private bool _getLanguagesTested = false;

    void Start()
    {
        LogSystem.InstallDefaultReactors();

        //  Create credential and instantiate service
        Credentials credentials = new Credentials(_username, _password, _url);

        _languageTranslator = new LanguageTranslator(credentials);
        _forcedGlossaryFilePath = Application.dataPath + "/Watson/Examples/ServiceExamples/TestData/glossary.tmx";

        Runnable.Run(Examples());
    }

    private IEnumerator Examples()
    {
        if (!_languageTranslator.GetTranslation(_pharseToTranslate, "en", "es", OnGetTranslation))
            Log.Debug("TestLanguageTranslator", "Failed to translate.");
        while (!_getTranslationTested)
            yield return null;

        if (!_languageTranslator.GetModels(OnGetModels))
            Log.Debug("TestLanguageTranslator", "Failed to get models.");
        while (!_getModelsTested)
            yield return null;

        if (!_languageTranslator.CreateModel(OnCreateModel, _baseModelName, _customModelName, _forcedGlossaryFilePath))
            Log.Debug("TestLanguageTranslator", "Failed to create model.");
        while (!_createModelTested)
            yield return null;

        if (!_languageTranslator.GetModel(OnGetModel, _customLanguageModelId))
            Log.Debug("TestLanguageTranslator", "Failed to get model.");
        while (!_getModelTested)
            yield return null;

        if (!_languageTranslator.DeleteModel(OnDeleteModel, _customLanguageModelId))
            Log.Debug("TestLanguageTranslator", "Failed to delete model.");
        while (!_deleteModelTested)
            yield return null;

        if (!_languageTranslator.Identify(OnIdentify, _pharseToIdentify))
            Log.Debug("TestLanguageTranslator", "Failed to identify language.");
        while (!_identifyTested)
            yield return null;

        if (!_languageTranslator.GetLanguages(OnGetLanguages))
            Log.Debug("TestLanguageTranslator", "Failed to get languages.");
        while (!_getLanguagesTested)
            yield return null;

        Log.Debug("TestLanguageTranslator", "Language Translator examples complete.");
    }

    private void OnGetModels(TranslationModels models, RESTConnector.Error error, string customData)
    {
        Log.Debug("TestLanguageTranslator", "Language Translator - Get models response: {0}", customData);
        _getModelsTested = true;
    }

    private void OnCreateModel(TranslationModel model, RESTConnector.Error error, string customData)
    {
        Log.Debug("TestLanguageTranslator", "Language Translator - Create model response: {0}", customData);
        _customLanguageModelId = model.model_id;
        _createModelTested = true;
    }

    private void OnGetModel(TranslationModel model, RESTConnector.Error error, string customData)
    {
        Log.Debug("TestLanguageTranslator", "Language Translator - Get model response: {0}", customData);
        _getModelTested = true;
    }

    private void OnDeleteModel(bool success, RESTConnector.Error error, string customData)
    {
        Log.Debug("TestLanguageTranslator", "Language Translator - Delete model response: success: {0}", success);
        _customLanguageModelId = null;
        _deleteModelTested = true;
    }

    private void OnGetTranslation(Translations translation, RESTConnector.Error error, string customData)
    {
        Log.Debug("TestLanguageTranslator", "Langauge Translator - Translate Response: {0}", customData);
        _getTranslationTested = true;
    }

    private void OnIdentify(string lang, RESTConnector.Error error, string customData)
    {
        Log.Debug("TestLanguageTranslator", "Language Translator - Identify response: {0}", customData);
        _identifyTested = true;
    }

    private void OnGetLanguages(Languages languages, RESTConnector.Error error, string customData)
    {
        Log.Debug("TestLanguageTranslator", "Language Translator - Get languages response: {0}", customData);
        _getLanguagesTested = true;
    }
}
