/**
* DevExpress HTML/JS Analytics Core (dx-analytics-core.d.ts)
* Version: 19.1.6
* Build date: 2019-09-10
* Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
* License: https://www.devexpress.com/Support/EULAs/NetComponents.xml
*/

import 'jquery';
import 'jqueryui';
import * as ko from 'knockout'
import dxButton from 'devextreme/ui/button';
import dxTextBox from 'devextreme/ui/text_box';
import dxDropDownBox from 'devextreme/ui/drop_down_box';
import dxPopup from 'devextreme/ui/popup';
import { IOptions as dxPopupOptions } from 'devextreme/ui/popup';
import ArrayStore from 'devextreme/data/array_store';
import CustomStore from 'devextreme/data/custom_store';
import DataSource from 'devextreme/data/data_source';

declare module DevExpress.Analytics {
    module Internal {
        function propertiesVisitor(target: any, visitor: (properties: any[]) => any, visited?: any[], skip?: Array<string>): void;
        function checkModelReady(model: any): boolean;
    }
    module Utils {
        interface IModelReady {
            isModelReady: ko.Computed<boolean>;
        }
        class UndoEngine extends Disposable {
            static _disposeUndoEngineSubscriptionsName: string;
            static tryGetUndoEngine(object: any): UndoEngine;
            private _groupObservers;
            private _getInfoMethodName;
            private _ignoredProperties;
            private _groupPosition;
            private _observers;
            private _subscriptions;
            private _targetSubscription;
            private _visited;
            private _position;
            private _lockedPosition;
            private _inUndoRedo;
            private _model;
            private readonly _modelReady;
            private _disposeObserver;
            private properyChanged;
            private visitProperties;
            private undoChangeSet;
            private redoChangeSet;
            private _disposeChilds;
            private _createDisposeFunction;
            private _callDisposeFunction;
            private _cleanSubscribtions;
            protected validatePropertyName(target: any, propertyName: string): string;
            subscribeProperty(property: any, subscribeChilds: any): ko.Subscription;
            subscribeProperties(properties: any): void;
            subscribe(target: any, info?: any): any[];
            private _removePropertiesSubscriptions;
            constructor(target: any, ignoredProperties?: string[], getInfoMethodName?: string);
            redoEnabled: ko.Observable<boolean>;
            undoEnabled: ko.Observable<boolean>;
            dispose(): void;
            removeTargetSubscription(): void;
            undoAll(): void;
            reset(): void;
            clearHistory(): void;
            undo(): void;
            redo(): void;
            isIngroup: number;
            isDirty: ko.Computed<boolean>;
            start(): void;
            end(): void;
        }
    }
}

declare module DevExpress.Analytics {
    module Internal {
        function _defineProperty(legacyObject: any, realObject: any, propertyName: any, newPropertyName?: any): void;
        function _definePropertyByString(rootObject: any, ...objectPathes: string[]): void;
        function addDisposeCallback(element: Node, callback: () => any): void;
    }
    module Utils {
        interface IDisposable {
            dispose: () => void;
            _disposables?: Array<ko.Subscription | ko.ComputedFunctions | IDisposable>;
        }
        class Disposable implements IDisposable {
            _disposables: Array<ko.Subscription | ko.ComputedFunctions | IDisposable>;
            isDisposing: boolean;
            constructor();
            disposeObservableArray(array: ko.ObservableArray<IDisposable>): void;
            resetObservableArray(array: ko.ObservableArray<any>): void;
            disposeArray(array: IDisposable[]): void;
            dispose(): void;
            removeProperties(): void;
        }
        function deserializeArray<T>(model: any, creator: (item: any) => any): ko.ObservableArray<T>;
        function serializeDate(date: Date): string;
    }
    module Internal {
        function knockoutArrayWrapper<T>(items?: any, ...onChange: Array<(array: any[], event?: string) => void>): ko.ObservableArray<T>;
        function isPlainObject(obj: any): boolean;
        function isEmptyObject(obj: any): boolean;
        function extend(target: any, object1?: any, ...objectN: any[]): any;
        function getPropertyValues(target?: any): any[];
    }
    module Utils {
        interface IEditorInfo {
            header?: string;
            content?: string;
            custom?: string;
            editorType?: any;
            extendedOptions?: any;
        }
        interface ISerializationInfo {
            propertyName: string;
            modelName?: string;
            defaultVal?: any;
            type?: ISerializableModelConstructor;
            info?: ISerializationInfoArray;
            from?: (val: any, serializer?: IModelSerializer) => any;
            toJsonObject?: any;
            array?: boolean;
            link?: boolean;
            editor?: IEditorInfo;
            displayName?: string;
            values?: {
                [key: string]: string;
            } | ko.Observable<{
                [key: string]: string;
            }>;
            valuesArray?: Array<IDisplayedValue>;
            initialize?: (viewModel: any, serilizer?: IModelSerializer) => void;
            validationRules?: Array<any>;
            validatorOptions?: any;
            editorOptions?: any;
            localizationId?: string;
            visible?: any;
            disabled?: any;
            valueStore?: any;
            addHandler?: () => any;
            alwaysSerialize?: boolean;
            template?: string;
            beforeSerialize?: (value: any) => any;
        }
        interface IDisplayedValue {
            value: any;
            displayValue: string;
            localizationId?: string;
        }
        interface ISerializationInfoArray extends Array<ISerializationInfo> {
        }
        interface ISerializableModel {
            _model?: any;
            getInfo?: () => ISerializationInfoArray;
        }
        interface ISerializableModelConstructor extends ISerializableModel {
            new (model?: any, serializer?: IModelSerializer, info?: ISerializationInfoArray): any;
        }
        interface IModelSerializerOptions {
            useRefs?: boolean;
            serializeDate?: (date: Date) => string;
        }
        interface IModelSerializer {
            deserialize(viewModel: ISerializableModel, model: any, serializationsInfo?: ISerializationInfoArray): any;
            serialize(viewModel: ISerializableModel, serializationsInfo?: ISerializationInfoArray, refs?: any): any;
        }
        class ModelSerializer implements IModelSerializer {
            private _options;
            private _refTable;
            private _linkTable;
            private linkObjects;
            constructor(options?: IModelSerializerOptions);
            deserializeProperty(modelPropertyInfo: ISerializationInfo, model: any): any;
            deserialize(viewModel: Utils.ISerializableModel, model: any, serializationsInfo?: Utils.ISerializationInfoArray): void;
            serialize(viewModel: Utils.ISerializableModel, serializationsInfo?: Utils.ISerializationInfoArray, refs?: any): any;
            private _isSerializableValue;
            private _serialize;
        }
        interface IEventHandler {
            type: any;
            value: (ev: any) => any;
        }
        class EventManager<Sender, EventType> extends Utils.Disposable {
            dispose(): void;
            private _handlers;
            call<K extends keyof EventType>(type: K, args: EventType[K]): void;
            addHandler<K extends keyof EventType>(type: K, listener: (this: Sender, ev: EventType[K]) => any): void;
            removeHandler<K extends keyof EventType>(type: K, listener: (this: Sender, ev: EventType[K]) => any): void;
        }
    }
}

declare module DevExpress.Analytics {
    module Internal {
        var removeWinSymbols: boolean;
        var Globalize: any;
        var messages: {};
        var custom_localization_values: {};
        function selectPlaceholder(): any;
        function noDataText(): any;
        function resolveFromPromises<T>(promises: JQueryPromise<any>[], createModel: () => T): JQueryDeferred<T>;
        function applyLocalizationToDevExtreme(currentCulture: string): void;
        function isCustomizedWithUpdateLocalizationMethod(text: string): boolean;
        function localize(val: string): any;
        function formatDate(val: any): any;
        function parseDate(val: any, useDefault?: boolean, format?: string): Date;
    }
    module Utils {
        function addCultureInfo(json: {
            messages: any;
        }): void;
        function _stringEndsWith(string: string, searchString: string): boolean;
        function getLocalization(text: string, id?: string, _removeWinSymbols?: boolean): any;
        function updateLocalization(object: {
            [key: string]: string;
        }): void;
    }
    module Localization {
        function loadMessages(messages: {
            [key: string]: string;
        }): void;
    }
    module Internal {
        var StringId: {
            MasterDetailRelationsEditor: string;
            DataAccessBtnOK: string;
            DataAccessBtnCancel: string;
            DataSourceWizardTitle: string;
            WizardPageConfigureQuery: string;
        };
        interface ILocalizationInfo {
            text: string;
            localizationId: string;
        }
        interface IFileUploadOptions {
            accept?: string;
            type?: string;
            readMode?: string;
        }
        interface IFileUploadResult {
            content: string;
            format: string;
        }
        function uploadFile(options: IFileUploadOptions): JQueryPromise<IFileUploadResult>;
        function compareEditorInfo(editor1: any, editor2: any): boolean;
        function findMatchesInString(textToTest: string, searchPattern: string): RegExpMatchArray;
        function escapeToRegExp(string: String): string;
        function formatUnicorn(text: string, ...args: any[]): string;
        interface IModelAction {
            action: (propertyName: string) => void;
            title: string;
            visible: (propertyName: string) => boolean;
        }
        interface IControlPropertiesViewModel {
            isPropertyDisabled: (name: string) => boolean;
            isPropertyVisible: (name: string) => boolean;
            isPropertyModified: (name: string) => boolean;
            controlType?: string;
            actions: IModelAction[];
            getInfo?: () => Analytics.Utils.ISerializationInfoArray;
        }
        interface IUndoEngine {
            start: () => void;
            end: () => void;
        }
    }
    module Widgets {
        var editorTemplates: any;
        module Internal {
            var propertiesGridEditorsPaddingLeft: number;
        }
        class Editor extends Utils.Disposable {
            _setPadding(position: string, value: any): {};
            _model: ko.Observable<Analytics.Internal.IControlPropertiesViewModel>;
            isVisibleByContent: ko.Observable<boolean>;
            isSearchedProperty: ko.Observable<boolean> | ko.Computed<boolean>;
            isParentSearched: ko.Observable<boolean>;
            rtl: boolean;
            private _validator;
            dispose(): void;
            constructor(modelPropertyInfo: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Observable<boolean> | ko.Computed<boolean>, textToSearch?: any);
            private _cachedValue;
            private _assignValue;
            private _init;
            findInfo(viewModel: any): any;
            updateInfo(propertyInfo: Analytics.Utils.ISerializationInfo): boolean;
            update(viewModel: Analytics.Internal.IControlPropertiesViewModel): void;
            getOptions(templateOptions: any): any;
            getValidatorOptions(templateValidatorOptions: any): any;
            _getEditorValidationRules(): any[];
            getValidationRules(): any;
            readonly validationRules: any;
            padding: any;
            level: any;
            textToSearch: ko.Observable<string> | ko.Computed<string>;
            info: ko.Observable<Analytics.Utils.ISerializationInfo>;
            name: string;
            displayName: ko.Computed<string>;
            templateName: string;
            contentTemplateName: string;
            editorTemplate: string;
            viewmodel: any;
            values: ko.Computed<{
                displayValue: string;
                value: string;
            }[]>;
            value: ko.Computed<any>;
            isEditorSelected: ko.Observable<boolean>;
            isPropertyModified: ko.Computed<boolean>;
            disabled: ko.Computed<boolean>;
            visible: ko.Computed<boolean>;
            getPopupServiceActions(): Analytics.Internal.IModelAction[];
            editorOptions: any;
            validatorOptions: any;
            defaultValue: any;
            readonly isComplexEditor: boolean;
            collapsed: ko.Observable<boolean>;
        }
        class PropertyGridEditor extends Editor {
            constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
            createObjectProperties(): ObjectProperties;
            visibleByName: ko.Computed<boolean>;
            editorCreated: ko.Observable<boolean>;
        }
        class EditorValidator extends Utils.Disposable {
            private _editor;
            private _lastValidatorOptions;
            private _lastModelOverridableRules;
            private _validatorInstance;
            private _onValidatedHandler;
            dispose(): void;
            constructor(_editor: Editor);
            _isValid(validationRules: any, newValue: any): {
                brokenRule?: any;
                isValid?: boolean;
                validationRules?: Array<any>;
                value?: any;
            };
            validatorInstance: any;
            onValidatedHandler: any;
            getValidationRules(): any;
            getValidatorOptions(templateValidatorOptions?: any): any;
            areRulesChanged(overridableRuleSet: Array<{
                type: string;
                message: any;
                validationCallback?: any;
            }>): number | boolean;
            wrapOnValidatorInitialized(options: any): void;
            _onValidatorInitialized(e: any): void;
            _concatValidationRules(validatorOptions: any, validationRules: any): any;
            _wrapValidatorEvents(validatorOptions: any): any;
            assignWithValidation(newValue: any, assignValueFunc: () => void): void;
        }
        class ObjectProperties extends Utils.Disposable {
            private _targetSubscription;
            private _infoSubscription;
            private _getInfoComputed;
            update(viewModel: Analytics.Internal.IControlPropertiesViewModel): void;
            private _cleanEditorsSubscriptions;
            dispose(): void;
            cleanSubscriptions(): void;
            cleanEditors(): void;
            findEditorByInfo(serializationInfo: Analytics.Utils.ISerializationInfo): Editor;
            createEditor(modelPropertyInfo: Analytics.Utils.ISerializationInfo): any;
            createEditors(serializationInfo: Analytics.Utils.ISerializationInfoArray): any[];
            private _createEditors;
            protected _update(target: any, editorsInfo: any, recreateEditors: any): void;
            constructor(target: ko.Observable<any> | ko.Computed<any>, editorsInfo?: {
                editors?: Analytics.Utils.ISerializationInfoArray | ko.Observable<Analytics.Utils.ISerializationInfoArray> | ko.Computed<Analytics.Utils.ISerializationInfoArray>;
            }, level?: number, parentDisabled?: ko.Observable<boolean> | ko.Computed<boolean>, recreateEditors?: boolean, textToSearch?: any);
            level: number;
            rtl: boolean;
            getEditors(): Editor[];
            _textToSearch: any;
            visible: ko.Observable<boolean> | ko.Computed<boolean>;
            _editors: ko.ObservableArray<Editor>;
            private _parentDisabled;
        }
    }
    module Internal {
        class CodeResolver {
            private _queue;
            private _done;
            done(callback: any): void;
            execute(func: any, time?: number): JQueryPromise<{}>;
        }
        var globalResolver: CodeResolver;
    }
    module Widgets {
    }
    module Internal {
        class PopupService {
            data: ko.Observable<any>;
            title: ko.Observable<string>;
            visible: ko.Observable<boolean>;
            actions: ko.ObservableArray<IModelAction>;
            target: ko.Observable<any>;
        }
        interface IEditorAddon {
            templateName: string;
            data: EditorAddOn;
        }
        class EditorAddOn extends Utils.Disposable {
            private _popupService;
            private _editor;
            private _updateActions;
            constructor(editor: Widgets.Editor, popupService: PopupService);
            showPopup: (_: any, element: any) => void;
            visible: ko.Computed<boolean>;
            editorMenuButtonCss: ko.Observable<string> | ko.Computed<string>;
            templateName: string;
        }
    }
    module Widgets {
        class GuidEditor extends Editor {
            _getEditorValidationRules(): any[];
        }
        module Internal {
            function validateGuid(guid: any): boolean;
            function validateNullableGuid(guid: any): boolean;
            var guidValidationRules: {
                type: string;
                validationCallback: (options: any) => boolean;
                message: () => any;
            }[];
            var guidRequiredValidationRules: {
                type: string;
                message: () => any;
            }[];
            var requiredValidationRules: {
                type: string;
                message: () => any;
            }[];
        }
        module Internal {
            class CollectionItemWrapper {
                constructor(editor: any, array: any, index: ko.Observable<number> | ko.Computed<number>, displayNameField?: string);
                editor: any;
                index: ko.Observable<number> | ko.Computed<number>;
                value: ko.Observable | ko.Computed;
                collapsed: ko.Observable<boolean> | ko.Computed<boolean>;
                name: ko.Observable<string> | ko.Computed<string>;
                selected: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            interface ICollectionEditorOptions {
                values: ko.Observable<ko.ObservableArray<any>> | ko.Computed<ko.ObservableArray<any>>;
                addHandler: () => any;
                displayName?: string;
                displayPropertyName?: string;
                hideButtons?: any;
                collapsed?: boolean;
                undoEngine?: ko.Observable<Analytics.Internal.IUndoEngine> | ko.Computed<Analytics.Internal.IUndoEngine>;
                level?: number;
                info?: ko.Observable<Analytics.Utils.ISerializationInfo> | ko.Computed<Analytics.Utils.ISerializationInfo>;
                template?: string;
                editorTemplate?: string;
                textEmptyArray?: Analytics.Internal.ILocalizationInfo;
                isVisibleButton?: (index: any, buttonName: any) => boolean;
                isDisabledButton?: (index: any, buttonName: any) => boolean;
            }
            class CollectionEditorViewModel {
                private _textEmptyArray;
                private _move;
                options: any;
                displayPropertyName: string;
                constructor(options: ICollectionEditorOptions, disabled?: ko.Observable<boolean>);
                getDisplayTextButton(key: string): any;
                getDisplayTextEmptyArray(): any;
                createCollectionItemWrapper(grandfather: any, index: any): CollectionItemWrapper;
                buttonMap: {
                    [keyname: string]: Analytics.Internal.ILocalizationInfo;
                };
                isVisibleButton: (buttonName: any) => boolean;
                isDisabledButton: (buttonName: any) => boolean;
                padding: number;
                values: ko.Observable<any[]> | ko.Computed<any[]>;
                add: (model: any) => void;
                up: (model: any) => void;
                down: (model: any) => void;
                remove: (model: any) => void;
                select: (event: any) => void;
                selectedIndex: ko.Observable<number>;
                collapsed: ko.Observable<boolean> | ko.Computed<boolean>;
                displayName: string;
                alwaysShow: ko.Observable<boolean>;
                showButtons: ko.Computed<boolean>;
                disabled: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            class dxEllipsisEditor extends dxTextBox {
                _$button: JQuery;
                _$buttonIcon: JQuery;
                _modelByElement: any;
                _koContext: any;
                constructor(element: any, options?: any);
                _init(): void;
                _render(): void;
                _renderButton(): void;
                _updateButtonSize(): void;
                _attachButtonEvents(): void;
                _optionChanged(obj: any, value: any): void;
            }
            class dxFileImagePicker extends dxEllipsisEditor {
                _filesinput: any;
                constructor(element: any, options?: any);
                _handleFiles(filesHolder: {
                    files: any;
                }): void;
                _$getInput(): JQuery;
                _render(): void;
                _renderInput(inputContainer: any): void;
                _attachButtonEvents(): void;
                _renderValue(): void;
            }
            var availableUnits: {
                value: string;
                displayValue: string;
                localizationId: string;
            }[];
            class FontModel extends Utils.Disposable {
                updateModel(value: string): void;
                updateValue(value: any): void;
                constructor(value: ko.Observable<string> | ko.Computed<string>);
                family: ko.Observable<string>;
                unit: ko.Observable<string>;
                isUpdateModel: boolean;
                size: ko.Observable<number>;
                modificators: {
                    bold: ko.Observable<boolean>;
                    italic: ko.Observable<boolean>;
                    strikeout: ko.Observable<boolean>;
                    underline: ko.Observable<boolean>;
                };
            }
            var availableFonts: ko.Observable<{
                [key: string]: string;
            }>;
        }
        class FontEditor extends PropertyGridEditor {
            constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
            createObjectProperties(): ObjectProperties;
        }
        module Metadata {
            var fontInfo: Utils.ISerializationInfoArray;
        }
        module Internal {
            var extendedTemplates: (templates?: {
                [key: string]: string;
            }) => {
                [key: string]: string;
            };
            var availableTemplates: {
                [key: string]: string;
            };
            class SvgTemplatesEngine {
                private static _instance;
                private _data;
                private _templates;
                private _hasTemplate;
                constructor();
                private static readonly Instance;
                static readonly templates: {
                    [key: string]: string;
                };
                static addTemplates(templates: any): void;
                static getExistingTemplate(name: any): any;
            }
            function getTemplate(id: string): string;
        }
    }
}

declare module DevExpress.Analytics {
    module Utils {
        interface IPathRequest {
            fullPath: string;
            path: string;
            ref?: string;
            id?: string;
            dataSource?: any;
            state?: any;
            pathParts?: string[];
        }
        class PathRequest implements IPathRequest {
            pathParts: string[];
            constructor(fullPath: string, pathParts?: string[]);
            fullPath: string;
            ref: string;
            id: string;
            path: string;
        }
    }
    module Widgets {
        module Internal {
            class ValueEditorHelper {
                private static _getCharFromKeyCode;
                private static _getCaretPosition;
                static editors: {
                    "integer": {
                        regExpEditing: RegExp;
                    };
                    "float": {
                        regExpEditing: RegExp;
                    };
                    "System.Byte": {
                        regExpEditing: RegExp;
                        minValue: any;
                        maxValue: string;
                    };
                    "System.SByte": {
                        regExpEditing: RegExp;
                        minValue: string;
                        maxValue: string;
                    };
                    "System.Int16": {
                        regExpEditing: RegExp;
                        minValue: string;
                        maxValue: string;
                    };
                    "System.UInt16": {
                        regExpEditing: RegExp;
                        minValue: any;
                        maxValue: string;
                    };
                    "System.Int32": {
                        regExpEditing: RegExp;
                        minValue: string;
                        maxValue: string;
                    };
                    "System.UInt32": {
                        regExpEditing: RegExp;
                        minValue: any;
                        maxValue: string;
                    };
                    "System.Int64": {
                        regExpEditing: RegExp;
                        minValue: string;
                        maxValue: string;
                    };
                    "System.UInt64": {
                        regExpEditing: RegExp;
                        minValue: any;
                        maxValue: string;
                    };
                    "System.Single": {
                        regExpEditing: RegExp;
                        minValue: string;
                        maxValue: string;
                    };
                    "System.Double": {
                        regExpEditing: RegExp;
                        minValue: string;
                        maxValue: string;
                    };
                    "System.Decimal": {
                        regExpEditing: RegExp;
                        minValue: string;
                        maxValue: string;
                    };
                };
                private static _validate;
                static validateWidgetValue(e: any, validate: (value: string) => boolean, defaultVal: string): void;
                static getNumberEditorOptions(id: string, specifics: string, extendOptions?: {}): any;
                static getValueEditorOptions(regExpEditing: RegExp, validate: (value: string) => boolean, defaultVal: string, extendOptions?: {}): any;
                static isValid(id: string, specifics: string, value: string): boolean;
                private static _invokeStandardHandler;
            }
        }
    }
    module Internal {
        function integerValueConverter(val: any, defaultValue: any): any;
        interface IValidateExpressionOptions {
            fieldListProvider: Utils.IItemsProvider;
            expression: string;
            path: string;
            rootItems?: string[];
        }
        function validateExpression(options: IValidateExpressionOptions): JQueryPromise<{}>;
        function floatValueConverter(val: any, defaultValue: any): any;
        function isDarkTheme(theme?: string): boolean;
        function setCursorInFunctionParameter(paramCount: any, editor: any, insertValue: any): void;
        function isList(data: Utils.IDataMemberInfo): boolean;
        function getParentContainer(el: HTMLElement, container?: string): JQuery;
        function isNullOrEmptyString(str: string): boolean;
    }
    module Utils {
        interface IDataMemberInfo {
            name: string;
            displayName: string;
            isList?: boolean;
            specifics?: string;
            isSelected?: boolean;
            dataType?: string;
            templateName?: string;
            innerActions?: any;
            noDragable?: any;
            dragData?: any;
            icon?: string;
        }
        interface IItemsProvider {
            getItems: (path: IPathRequest) => JQueryPromise<IDataMemberInfo[]>;
            getItemByPath?: (path: IPathRequest) => JQueryPromise<IDataMemberInfo>;
            getValues?: (path: IPathRequest) => JQueryPromise<any[]>;
        }
        interface IHotKey {
            ctrlKey: boolean;
            keyCode: number;
        }
        interface IAction {
            id?: string;
            text: string;
            textId?: string;
            displayText?: () => string;
            imageClassName: any;
            imageTemplateName?: any;
            container?: string;
            clickAction: (model?: any) => void;
            disabled?: ko.Observable<boolean> | ko.Computed<boolean>;
            hotKey?: IHotKey;
            hasSeparator?: boolean;
            templateName?: string;
            visible?: any;
            zoomStep?: any;
            zoomLevels?: any;
            zoom?: any;
            zoomItems?: any;
            position?: number;
            selected?: ko.Observable<boolean> | ko.Computed<boolean>;
            displayExpr?: (val: any) => string;
            onCustomItemCreating?: (e: {
                text: string;
                customItem: any;
            }) => void;
        }
    }
    module Widgets {
        module Internal {
            interface IAceEditor {
                require(module: string): any;
                edit(element: HTMLElement): any;
            }
            var ace: IAceEditor;
            var aceAvailable: boolean;
        }
        module Internal {
            interface IExpressionEditorItem {
                text: string;
                description?: string;
                descriptionStringId?: string;
            }
            interface IExpressionEditorOperatorItem extends IExpressionEditorItem {
                image?: string;
                hasSeparator?: boolean;
            }
            var operatorNames: Array<IExpressionEditorOperatorItem>;
            interface IExpressionEditorFunctionItem extends IExpressionEditorItem {
                paramCount: number;
                displayName?: string;
            }
            interface IExpressionEditorFunction {
                display: string;
                localizationId?: string;
                items?: {
                    [key: string]: Array<IExpressionEditorFunctionItem>;
                };
                category?: string;
            }
            var functionDisplay: Array<IExpressionEditorFunction>;
            function insertInFunctionDisplay(addins: any): Array<Internal.IExpressionEditorFunction>;
        }
    }
    module Criteria {
        class CriteriaOperator {
            static operators(enums: any): any;
            static parse(stringCriteria: string): CriteriaOperator;
            static create(operatorType: any): CriteriaOperator;
            static and(left: CriteriaOperator, right: CriteriaOperator): CriteriaOperator;
            static or(left: CriteriaOperator, right: CriteriaOperator): CriteriaOperator;
            static getNotValidRange(value: string, errorMessage: string): {
                start: number;
                end: number;
            };
            readonly displayType: string;
            readonly enumType: any;
            operatorType: any;
            type: string;
            operands: any;
            create: (operatorType: any, field: CriteriaOperator) => CriteriaOperator;
            remove: (operand: CriteriaOperator) => void;
            change: (operandType: any, operand: CriteriaOperator, incorrectSpecificsForAggregate: boolean) => CriteriaOperator;
            changeValueType: (type: any, location: Utils.IPropertyLocation) => CriteriaOperator;
            changeValue: (operand: CriteriaOperator, reverse: boolean, location: Utils.IPropertyLocation) => CriteriaOperator;
            assignLeftPart: (criteriaOperator: any) => any;
            assignRightPart: (criteriaOperator: any) => any;
            assignType: (type: any) => void;
            readonly leftPart: any;
            readonly rightPart: any;
            resetrightPart: (value: any) => any;
            assignFrom(criteriaOperator: any, incorrectSpecificsForAggregate?: boolean): void;
            children(): any[];
            accept(visitor: Utils.ICriteriaOperatorVisitor): CriteriaOperator;
        }
    }
    module Widgets {
        module Internal {
            interface ICompletionRootItem {
                name: string;
                needPrefix?: boolean;
                rootPath?: string;
            }
            interface ICodeCompletorOptions {
                editor: any;
                bindingContext: any;
                fieldListProvider: Utils.IItemsProvider;
                path: ko.Observable<string> | ko.Computed<string>;
                functions?: Array<Internal.IExpressionEditorFunction> | ko.ObservableArray<Internal.IExpressionEditorFunction>;
                rootItems?: Array<ICompletionRootItem>;
                getRealExpression?: (path: string, member: string) => JQueryPromise<string>;
            }
            class CodeCompletor extends Utils.Disposable {
                private _options;
                private _fieldListProvider;
                private _path;
                private _editor;
                private _contextPath;
                private _functions;
                private _rootItems;
                private _isInContext;
                private _getPath;
                private _previousSymbol;
                beforeInsertMatch(editor: any, token: any, parentPrefix: any): void;
                insertMatch(editor: any, parentPrefix: any, fieldName: any): void;
                generateFieldDisplayName(parentPrefix: any, displayName: any): string;
                private _convertDataMemberInfoToCompletions;
                private _combinePath;
                private _getParentPrefix;
                private _getRealPath;
                private _getFields;
                private static _cleanupFields;
                getFunctionsCompletions(): any[];
                getAggregateCompletions(): any[];
                getOperatorCompletions(prefix: any): {
                    caption: string;
                    snippet: string;
                    meta: any;
                }[];
                private _addFunctions;
                private _addAggregates;
                private _addOperators;
                private _addParameterOperators;
                private _getOperands;
                private _getOperandsOrOperators;
                private _findStartContextTokenPosition;
                private _findOpenedStartContext;
                private _findOpenedAggregates;
                private _getContextPath;
                private _getCompletions;
                defaultProcess(token: any, text: any, completions: any): JQueryPromise<{}>;
                constructor(_options: ICodeCompletorOptions);
                identifierRegexps: RegExp[];
                getCompletions(aceEditor: any, session: any, pos: any, prefix: any, callback: any): void;
                getDocTooltip(item: any): void;
            }
            function createFunctionCompletion(fnInfo: Internal.IExpressionEditorFunctionItem, name: string, insertValue?: string): {
                caption: string;
                snippet: string;
                meta: any;
                tooltip: any;
                score: number;
                completer: {
                    insertMatch: (editor: any, data: any) => void;
                };
            };
        }
    }
    module Criteria {
        module Utils {
            interface IPropertyLocation {
                index?: number;
                name?: string;
            }
            var operatorTokens: {
                "Plus": string;
                "Minus": string;
                "Equal": string;
                "NotEqual": string;
                "Greater": string;
                "Less": string;
                "LessOrEqual": string;
                "GreaterOrEqual": string;
            };
            function criteriaForEach(operator: CriteriaOperator, callback: (operator: CriteriaOperator, path?: any) => void, path?: string): void;
            interface ICriteriaOperatorVisitor {
                visitGroupOperator?: (element: GroupOperator) => GroupOperator;
                visitOperandProperty?: (element: OperandProperty) => OperandProperty;
                visitConstantValue?: (element: ConstantValue) => ConstantValue;
                visitOperandParameter?: (element: OperandParameter) => OperandParameter;
                visitAggregateOperand?: (element: AggregateOperand) => AggregateOperand;
                visitJoinOperand?: (element: JoinOperand) => JoinOperand;
                visitBetweenOperator?: (element: BetweenOperator) => BetweenOperator;
                visitInOperator?: (element: InOperator) => InOperator;
                visitBinaryOperator?: (element: BinaryOperator) => BinaryOperator;
                visitUnaryOperator?: (element: UnaryOperator) => UnaryOperator;
                visitFunctionOperator?: (element: FunctionOperator) => FunctionOperator;
            }
        }
        interface IOperandPropertyOptions {
            propertyName: string;
            startColumn: any;
            startLine: any;
            originalPropertyLength: any;
        }
        class OperandProperty extends CriteriaOperator {
            constructor(propertyName?: string, startColumn?: number, startLine?: number, originalPropertyLength?: number);
            readonly displayType: string;
            propertyName: string;
            originalPropertyLength: number;
            type: string;
            startPosition: {
                line: number;
                column: number;
            };
            accept(visitor: Utils.ICriteriaOperatorVisitor): OperandProperty;
        }
    }
    module Utils {
        interface IDisplayExpressionConverter {
            toDisplayExpression(path: string, expression: string): JQueryPromise<string>;
            toRealExpression(path: string, expression: string): JQueryPromise<string>;
        }
        interface IDisplayNameProvider {
            getDisplayNameByPath: (path: string, dataMember: string) => JQueryPromise<string>;
            getRealName: (path: string, displayDataMember: string) => JQueryPromise<string>;
        }
    }
    module Internal {
        class DisplayExpressionConverter implements Utils.IDisplayExpressionConverter {
            private displayNameProvider;
            private _replaceNames;
            constructor(displayNameProvider: Utils.IDisplayNameProvider);
            toDisplayExpression(path: string, expression: string): JQueryPromise<string>;
            toRealExpression(path: string, expression: string): JQueryPromise<any>;
        }
    }
    module Criteria {
        class CriteriaOperatorPreprocessor {
            _func: Array<(currentOperand: CriteriaOperator, options: {
                operatorType: string;
                options: any;
            }) => CriteriaOperator>;
            CriteriaOperator(): CriteriaOperator;
            BinaryOperator(options: IBinaryOperatorOptions): BinaryOperator;
            FunctionOperator(options: IFunctionOperatorProperties): FunctionOperator;
            AggregateOperand(options: IAggregateOperandOptions): AggregateOperand;
            GroupOperator(options: IGroupOperatorOptions): GroupOperator;
            InOperator(options: IInOperatorOptions): InOperator;
            ConstantValue(options: IOperandValueOptions): ConstantValue;
            OperandValue(options: IOperandValueOptions): OperandValue;
            OperandParameter(options: IOperandParameterOptions): OperandParameter;
            OperandProperty(options: IOperandPropertyOptions): OperandProperty;
            UnaryOperator(options: IUnaryOperatorOptions): UnaryOperator;
            BetweenOperator(options: IBetweenOperatorOptions): BetweenOperator;
            JoinOperator(options: IJoinOperandOptions): JoinOperand;
            addListener(func: (currentOperand: CriteriaOperator, options: {
                operatorType: string;
                options: any;
            }) => CriteriaOperator): void;
            removeListener(func: (currentOperand: CriteriaOperator, options: {
                operatorType: string;
                options: any;
            }) => CriteriaOperator): void;
            process(operatorType: string, options: any): CriteriaOperator;
        }
        var criteriaCreator: CriteriaOperatorPreprocessor;
        enum Aggregate {
            Count = 0,
            Exists = 1,
            Min = 2,
            Max = 3,
            Avg = 4,
            Sum = 5,
            Single = 6
        }
        interface IAggregateOperandOptions {
            property: CriteriaOperator;
            aggregatedExpression: CriteriaOperator;
            aggregateType: Aggregate;
            condition: any;
        }
        class AggregateOperand extends CriteriaOperator {
            constructor(property: CriteriaOperator, aggregatedExpression: CriteriaOperator, aggregateType: Aggregate, condition: any);
            readonly displayType: string;
            readonly enumType: typeof Aggregate;
            readonly leftPart: CriteriaOperator;
            children(): any[];
            change: (operationType: any, item: CriteriaOperator) => any;
            assignLeftPart: (criteriaOperator: any) => void;
            property: CriteriaOperator;
            condition: CriteriaOperator;
            operatorType: Aggregate;
            aggregatedExpression: any;
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): AggregateOperand;
        }
        interface IBetweenOperatorOptions {
            property: CriteriaOperator;
            begin: CriteriaOperator;
            end: CriteriaOperator;
        }
        class BetweenOperator extends CriteriaOperator {
            constructor(property: CriteriaOperator, begin: CriteriaOperator, end: CriteriaOperator);
            property: CriteriaOperator;
            begin: CriteriaOperator;
            end: CriteriaOperator;
            readonly leftPart: CriteriaOperator;
            readonly rightPart: CriteriaOperator[];
            resetRightPart: (newVal: any) => void;
            assignLeftPart: (criteriaOperator: any) => void;
            assignRightPart: (criteriaOperator: any) => void;
            readonly displayType: string;
            operatorType: string;
            readonly enumType: typeof BetweenOperator;
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): BetweenOperator;
        }
        enum BinaryOperatorType {
            Equal = 0,
            NotEqual = 1,
            Greater = 2,
            Less = 3,
            LessOrEqual = 4,
            GreaterOrEqual = 5,
            Like = 6,
            BitwiseAnd = 7,
            BitwiseOr = 8,
            BitwiseXor = 9,
            Divide = 10,
            Modulo = 11,
            Multiply = 12,
            Plus = 13,
            Minus = 14
        }
        interface IBinaryOperatorOptions {
            left: CriteriaOperator;
            right: CriteriaOperator;
            operatorType: BinaryOperatorType;
        }
        class BinaryOperator extends CriteriaOperator {
            constructor(left: CriteriaOperator, right: CriteriaOperator, operatorType: BinaryOperatorType);
            readonly leftPart: CriteriaOperator;
            readonly rightPart: CriteriaOperator;
            assignLeftPart: (criteriaOperator: any) => void;
            assignRightPart: (criteriaOperator: any) => void;
            leftOperand: CriteriaOperator;
            rightOperand: CriteriaOperator;
            operatorType: BinaryOperatorType;
            readonly displayType: any;
            readonly enumType: typeof BinaryOperatorType;
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): BinaryOperator;
        }
        interface IOperandValueOptions {
            value: any;
        }
        class OperandValue extends CriteriaOperator {
            private _processStringValue;
            constructor(value?: any);
            readonly displayType: any;
            value: any;
            type: string;
            specifics: string;
        }
        class ConstantValue extends OperandValue {
            constructor(value: any);
            accept(visitor: Utils.ICriteriaOperatorVisitor): ConstantValue;
        }
        enum FunctionOperatorType {
            None = 0,
            Custom = 1,
            CustomNonDeterministic = 2,
            Iif = 3,
            IsNull = 4,
            IsNullOrEmpty = 5,
            Trim = 6,
            Len = 7,
            Substring = 8,
            Upper = 9,
            Lower = 10,
            Concat = 11,
            Ascii = 12,
            Char = 13,
            ToStr = 14,
            Replace = 15,
            Reverse = 16,
            Insert = 17,
            CharIndex = 18,
            Remove = 19,
            Abs = 20,
            Sqr = 21,
            Cos = 22,
            Sin = 23,
            Atn = 24,
            Exp = 25,
            Log = 26,
            Rnd = 27,
            Tan = 28,
            Power = 29,
            Sign = 30,
            Round = 31,
            Ceiling = 32,
            Floor = 33,
            Max = 34,
            Min = 35,
            Acos = 36,
            Asin = 37,
            Atn2 = 38,
            BigMul = 39,
            Cosh = 40,
            Log10 = 41,
            Sinh = 42,
            Tanh = 43,
            PadLeft = 44,
            PadRight = 45,
            StartsWith = 46,
            EndsWith = 47,
            Contains = 48,
            ToInt = 49,
            ToLong = 50,
            ToFloat = 51,
            ToDouble = 52,
            ToDecimal = 53,
            LocalDateTimeThisYear = 54,
            LocalDateTimeThisMonth = 55,
            LocalDateTimeLastWeek = 56,
            LocalDateTimeThisWeek = 57,
            LocalDateTimeYesterday = 58,
            LocalDateTimeToday = 59,
            LocalDateTimeNow = 60,
            LocalDateTimeTomorrow = 61,
            LocalDateTimeDayAfterTomorrow = 62,
            LocalDateTimeNextWeek = 63,
            LocalDateTimeTwoWeeksAway = 64,
            LocalDateTimeNextMonth = 65,
            LocalDateTimeNextYear = 66,
            IsOutlookIntervalBeyondThisYear = 67,
            IsOutlookIntervalLaterThisYear = 68,
            IsOutlookIntervalLaterThisMonth = 69,
            IsOutlookIntervalNextWeek = 70,
            IsOutlookIntervalLaterThisWeek = 71,
            IsOutlookIntervalTomorrow = 72,
            IsOutlookIntervalToday = 73,
            IsOutlookIntervalYesterday = 74,
            IsOutlookIntervalEarlierThisWeek = 75,
            IsOutlookIntervalLastWeek = 76,
            IsOutlookIntervalEarlierThisMonth = 77,
            IsOutlookIntervalEarlierThisYear = 78,
            IsOutlookIntervalPriorThisYear = 79,
            IsThisWeek = 80,
            IsThisMonth = 81,
            IsThisYear = 82,
            DateDiffTick = 83,
            DateDiffSecond = 84,
            DateDiffMilliSecond = 85,
            DateDiffMinute = 86,
            DateDiffHour = 87,
            DateDiffDay = 88,
            DateDiffMonth = 89,
            DateDiffYear = 90,
            GetDate = 91,
            GetMilliSecond = 92,
            GetSecond = 93,
            GetMinute = 94,
            GetHour = 95,
            GetDay = 96,
            GetMonth = 97,
            GetYear = 98,
            GetDayOfWeek = 99,
            GetDayOfYear = 100,
            GetTimeOfDay = 101,
            Now = 102,
            UtcNow = 103,
            Today = 104,
            AddTimeSpan = 105,
            AddTicks = 106,
            AddMilliSeconds = 107,
            AddSeconds = 108,
            AddMinutes = 109,
            AddHours = 110,
            AddDays = 111,
            AddMonths = 112,
            AddYears = 113,
            OrderDescToken = 114
        }
        interface IFunctionOperatorProperties {
            operatorType: FunctionOperatorType;
            operands: any[];
        }
        class FunctionOperator extends CriteriaOperator {
            constructor(operatorType: FunctionOperatorType, operands: any);
            toString: (reverse: any) => string;
            operatorType: FunctionOperatorType;
            assignLeftPart: (criteriaOperator: any) => void;
            assignRightPart: (criteriaOperator: any) => void;
            readonly leftPart: any;
            readonly rightPart: any[];
            readonly displayType: string;
            readonly enumType: typeof FunctionOperatorType;
            operands: any[];
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): FunctionOperator;
        }
        enum GroupOperatorType {
            And = 0,
            Or = 1
        }
        interface IGroupOperatorOptions {
            operation: GroupOperatorType;
            operands: Array<CriteriaOperator>;
        }
        class GroupOperator extends CriteriaOperator {
            constructor(operation: GroupOperatorType, operands: Array<CriteriaOperator>);
            static combine(operation: GroupOperatorType, operands: Array<CriteriaOperator>): CriteriaOperator;
            create: (isGroup: any, property: OperandProperty, specifics?: string) => any;
            change: (operationType: any, item: any, incorrectSpecificsForAggregate?: boolean) => any;
            remove: (operator: CriteriaOperator) => void;
            operatorType: GroupOperatorType;
            assignLeftPart: (operator: CriteriaOperator) => void;
            children(): any[];
            readonly displayType: string;
            readonly enumType: typeof GroupOperatorType;
            operands: any[];
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): GroupOperator;
        }
        interface IInOperatorOptions {
            criteriaOperator: CriteriaOperator;
            operands: any[];
        }
        class InOperator extends CriteriaOperator {
            constructor(criteriaOperator: CriteriaOperator, operands: any);
            readonly leftPart: CriteriaOperator;
            readonly rightPart: any[];
            assignLeftPart: (criteriaOperator: any) => void;
            assignRightPart: (criteriaOperator: any) => void;
            criteriaOperator: CriteriaOperator;
            readonly displayType: string;
            operatorType: string;
            readonly enumType: typeof InOperator;
            type: string;
            operands: any[];
            accept(visitor: Utils.ICriteriaOperatorVisitor): InOperator;
        }
        interface IJoinOperandOptions {
            joinTypeName: string;
            condition: CriteriaOperator;
            type: Aggregate;
            aggregated: CriteriaOperator;
        }
        class JoinOperand extends CriteriaOperator {
            constructor(joinTypeName: string, condition: CriteriaOperator, type: Aggregate, aggregated: CriteriaOperator);
            static joinOrAggregate(collectionProperty: OperandProperty, condition: CriteriaOperator, type: Aggregate, aggregated: CriteriaOperator): CriteriaOperator;
            joinTypeName: string;
            condition: CriteriaOperator;
            operatorType: Aggregate;
            aggregatedExpression: CriteriaOperator;
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): JoinOperand;
        }
        interface IOperandParameterOptions {
            parameterName?: string;
            value?: string;
        }
        class OperandParameter extends OperandValue {
            constructor(parameterName?: string, value?: string);
            readonly displayType: string;
            parameterName: string;
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): OperandParameter;
        }
        enum UnaryOperatorType {
            Minus = 0,
            Plus = 1,
            BitwiseNot = 2,
            Not = 3,
            IsNull = 4
        }
        interface IUnaryOperatorOptions {
            operatorType: UnaryOperatorType;
            operator: CriteriaOperator;
        }
        class UnaryOperator extends CriteriaOperator {
            constructor(operatorType: UnaryOperatorType, operand: CriteriaOperator);
            readonly leftPart: CriteriaOperator;
            operand: CriteriaOperator;
            operatorType: UnaryOperatorType;
            assignFrom(criteriaOperator: any): void;
            readonly displayType: string;
            readonly enumType: typeof UnaryOperatorType;
            type: string;
            accept(visitor: Utils.ICriteriaOperatorVisitor): UnaryOperator;
        }
    }
    module Widgets {
        module Internal {
            interface IExpressionEditorContent {
                data: any;
                name: string;
                isSelected: ko.Observable<boolean> | ko.Computed<boolean>;
                showDescription: boolean;
            }
            interface IExpressionEditorCategory extends Utils.IDisposable {
                displayName: string;
                collapsed?: ko.Observable<boolean> | ko.Computed<boolean>;
                content?: IExpressionEditorContent;
                items?: ko.Observable<IExpressionEditorContent[]> | ko.Computed<IExpressionEditorContent[]>;
                templateName?: string;
            }
            class Tools extends Utils.Disposable {
                private _defaultClick;
                searchPlaceholder: () => any;
                private _generateTab;
                private _localizedExpressionEditorItem;
                private _initDescription;
                private _createFieldsCategory;
                private _createConstantCategory;
                private _createOperatorsCategory;
                private _createFunctionsCategoryContent;
                private _createFunctionsCategoryItem;
                private _createFunctionsCategory;
                private _disposeCategories;
                constructor(onClick: (item: any, element: any) => void, parametersOptions: any, options: ko.Observable<IExpressionOptions> | ko.Computed<IExpressionOptions>, fieldListOptions?: any);
                dispose(): void;
                resetCategoriesSelection: () => void;
                private _categories;
                showDescription: ko.Observable<boolean> | ko.Computed<boolean>;
                toolBox: any[];
                description: ko.Observable<string> | ko.Computed<string>;
            }
            var treeListEditAction: Utils.IAction;
            interface ITreeListOptions {
                itemsProvider: Utils.IItemsProvider;
                selectedPath: ko.Observable<string> | ko.Computed<string>;
                treeListController: ITreeListController;
                templateName?: string;
                rtl?: boolean;
                path?: ko.Observable<string> | ko.Observable<string[]>;
                onItemsVisibilityChanged?: () => void;
                expandRootItems?: boolean;
                pageSize?: number;
                templateHtml?: string;
            }
            class TreeListEllipsisButton {
                private _availableItemsCount;
                private padding;
                private pageSize;
                constructor(_availableItemsCount: ko.Observable<number>, padding: any, pageSize: number);
                templateName: string;
                collapsed: () => boolean;
                visibleItems: () => any[];
                text: () => any;
                renderNext(): void;
            }
            class TreeListItemViewModel extends Utils.Disposable {
                protected resolver: Analytics.Internal.CodeResolver;
                private _rtl;
                private _data;
                private _actions;
                private _pageSize;
                private _actionsSubscription;
                private _templateName;
                private _equal;
                private _treeListController;
                private _loader;
                private _iconName;
                private _getImageClassName;
                private _getImageTemplateName;
                private _getNodeImageClassName;
                private _createItemsObj;
                private _loadItems;
                protected _onItemsChanged(): void;
                _selectItem(itemPath: string): void;
                _find(itemPath: string): void;
                private _applyPadding;
                private _initPaginate;
                constructor(options: ITreeListOptions, path?: string[], onItemsVisibilityChanged?: () => any, rtl?: boolean, resolver?: Analytics.Internal.CodeResolver);
                dragDropHandler: any;
                dblClickHandler: any;
                _path: string[];
                _onItemsVisibilityChanged: () => void;
                level: number;
                parent: TreeListItemViewModel;
                padding: {};
                items: ko.ObservableArray<TreeListItemViewModel>;
                maxItemsCount: ko.Observable<number>;
                visibleItems: ko.Computed<any[]>;
                collapsed: ko.Observable<boolean>;
                nodeImageClass: ko.Computed<string>;
                imageClassName: ko.Computed<string>;
                imageTemplateName: ko.Computed<string>;
                readonly hasItems: boolean;
                data: Utils.IDataMemberInfo;
                readonly name: string;
                readonly path: string;
                readonly pathParts: string[];
                readonly text: string;
                readonly templateName: string;
                actionsTemplate(): any;
                treeListEditAction(): Utils.IAction;
                readonly hasContent: boolean;
                readonly actions: Utils.IAction[];
                readonly isDraggable: boolean;
                readonly treeListController: ITreeListController;
                toggleCollapsed: () => void;
                toggleSelected: any;
                isSelected: ko.Observable<boolean>;
                isHovered: ko.Observable<boolean>;
                isMultiSelected: ko.Observable<boolean>;
                getItems: () => JQueryPromise<TreeListItemViewModel[]>;
                dispose(): void;
                readonly visible: boolean;
                mouseenter(): void;
                mouseleave(): void;
                selectedItems(): TreeListItemViewModel[];
            }
            class TreeListRootItemViewModel extends TreeListItemViewModel {
                private _options;
                dispose(): void;
                private _selectedPathSubscription;
                constructor(_options: ITreeListOptions, path?: string[], onItemsVisibilityChanged?: () => any, rtl?: boolean);
                _onItemsChanged(): void;
            }
            interface ITreeListController {
                itemsFilter: (item: Utils.IDataMemberInfo, path?: string) => boolean;
                hasItems: (item: Utils.IDataMemberInfo) => boolean;
                canSelect: (value: TreeListItemViewModel) => boolean;
                select: (value: TreeListItemViewModel) => void;
                canMultiSelect?: (value: TreeListItemViewModel) => boolean;
                multiSelect?: (value: TreeListItemViewModel, isShiftPressed: boolean, isCtrlPressed: boolean) => void;
                getActions?: (item: TreeListItemViewModel) => Utils.IAction[];
                isDraggable?: (item: TreeListItemViewModel) => boolean;
                dblClickHandler?: (item: TreeListItemViewModel) => void;
                clickHandler?: (item: TreeListItemViewModel) => void;
                dragDropHandler?: any;
                selectedItems?: () => TreeListItemViewModel[];
                showIconsForChildItems?: (item?: TreeListItemViewModel) => boolean;
                searchName?: ko.Observable<string> | ko.Computed<string>;
                dispose?: () => void;
            }
            class TreeListController implements ITreeListController {
                dispose(): void;
                itemsFilter(item: Utils.IDataMemberInfo): boolean;
                hasItems(item: Utils.IDataMemberInfo): boolean;
                canSelect(value: TreeListItemViewModel): boolean;
                select(value: TreeListItemViewModel): void;
                selectedItem: any;
            }
            class ExpressionEditorTreeListController extends TreeListController {
                fieldName: any;
                putSelectionHandler: (item: TreeListItemViewModel, element: any) => void;
                selectionHandler?: (item: TreeListItemViewModel) => void;
                constructor(fieldName: any, putSelectionHandler: (item: TreeListItemViewModel, element: any) => void, selectionHandler?: (item: TreeListItemViewModel) => void);
                itemsFilter(item: Utils.IDataMemberInfo): boolean;
                select(value: Internal.TreeListItemViewModel): void;
                getActions(item: TreeListItemViewModel): {
                    clickAction: (element: any) => void;
                }[];
                canSelect(value: TreeListItemViewModel): boolean;
            }
            class ExpressionEditorParametersTreeListController extends TreeListController {
                customFilter: (item: Utils.IDataMemberInfo) => boolean;
                putSelectionHandler: (selectedItemPath: string, element: any) => void;
                selectionHandler?: (item: TreeListItemViewModel) => void;
                constructor(customFilter: (item: Utils.IDataMemberInfo) => boolean, putSelectionHandler: (selectedItemPath: string, element: any) => void, selectionHandler?: (item: TreeListItemViewModel) => void);
                itemsFilter(item: Utils.IDataMemberInfo): boolean;
                select(value: Internal.TreeListItemViewModel): void;
                getActions(item: TreeListItemViewModel): {
                    clickAction: (element: any) => void;
                }[];
                canSelect(value: TreeListItemViewModel): boolean;
            }
        }
        interface IExpressionOptions {
            value: ko.Observable<string> | ko.Computed<string>;
            path?: ko.Observable<string> | ko.Computed<string>;
            fieldName?: ko.Observable<string> | ko.Computed<string>;
            theme?: string;
            patchFieldName?: (fieldName: string) => string;
            functions?: Array<Internal.IExpressionEditorFunction>;
            rootItems?: Array<Internal.ICompletionRootItem>;
            customizeCategories?: (sender: any, categories: any, dblclick?: any) => void;
            validate?: (criteria: Criteria.CriteriaOperator) => boolean;
            isValid?: ko.Observable<boolean> | ko.Computed<boolean>;
        }
        module Internal {
            function createExpressionEditorCollectionToolOptions(collectionItems: any, toolName: string, displayToolName: string, showDescription: boolean): {
                displayName: any;
                content: {
                    showDescription: boolean;
                    isSelected: ko.Observable<boolean>;
                    data: {
                        items: any;
                        selectedItem: ko.Observable<any>;
                    };
                    name: string;
                };
                dispose: () => any;
            };
            function wrapExpressionValue(path: ko.Observable<string> | ko.Computed<string>, value: ko.Observable<string> | ko.Computed<string>, converter: Utils.IDisplayExpressionConverter, subscriptions: any): ko.Observable<string> | ko.Computed<string>;
        }
        class ExpressionEditor extends Utils.Disposable {
            private options;
            private _displayNameProvider?;
            dispose(): void;
            private _createMainPopupButtons;
            private _getTextArea;
            private _updateTextAreaValue;
            private _updateAceValue;
            private _updateValue;
            private patchFieldName;
            private _parametersPutSelectionHandler;
            private _fieldsPutSelectionHandler;
            private _createToolsOptions;
            constructor(options: IExpressionOptions, fieldListProvider: ko.Observable<Utils.IItemsProvider> | ko.Computed<Utils.IItemsProvider>, disabled?: ko.Observable<boolean> | ko.Computed<boolean>, rtl?: boolean, _displayNameProvider?: Utils.IDisplayNameProvider);
            displayExpressionConverter: Analytics.Internal.DisplayExpressionConverter;
            aceAvailable: boolean;
            tools: Internal.Tools;
            displayValue: ko.Observable<string> | ko.Computed<string>;
            popupVisible: ko.Observable<boolean>;
            title: () => any;
            value: ko.Observable<string> | ko.Computed<string>;
            textAreaValue: ko.Observable<string>;
            theme: string;
            languageHelper: {
                getLanguageMode: () => string;
                createCompleters: (editor: any, bindingContext: any, viewModel: ExpressionEditor) => Internal.CodeCompletor[];
            };
            aceOptions: {
                showLineNumbers: boolean;
                showPrintMargin: boolean;
                enableBasicAutocompletion: boolean;
                enableLiveAutocompletion: boolean;
                showFoldWidgets: boolean;
                highlightActiveLine: boolean;
            };
            additionalOptions: {
                onChange: (session: any) => void;
            };
            callbacks: {
                focus: () => any;
            };
            koOptions: ko.Observable<IExpressionOptions> | ko.Computed<IExpressionOptions>;
            editorContainer: ko.Observable<any> | ko.Computed<any>;
            fieldListProvider: ko.Observable<Utils.IItemsProvider> | ko.Computed<Utils.IItemsProvider>;
            parametersTreeListController: Internal.ExpressionEditorParametersTreeListController;
            save: (sender: any) => void;
            isValid: ko.Observable<boolean> | ko.Computed<boolean>;
            buttonItems: any[];
            rtl: boolean;
            modelValueValid: ko.Observable<boolean> | ko.Computed<boolean>;
            disabled: ko.Observable<boolean> | ko.Computed<boolean>;
            onShown(): void;
            getPopupContainer: typeof Analytics.Internal.getParentContainer;
        }
        module Filtering {
            class CriteriaOperatorSurface<T extends Criteria.CriteriaOperator> extends Utils.Disposable {
                _createLeftPartProperty(value: any): CriteriaOperatorSurface<Criteria.CriteriaOperator>;
                createChildSurface(item: any, path?: any, actions?: any): CriteriaOperatorSurface<Criteria.CriteriaOperator>;
                protected getDisplayType(): string;
                constructor(operator: T, parent: any, fieldListProvider: any, path: any);
                specifics: ko.Observable<string> | ko.Computed<string>;
                dataType: ko.Observable<string> | ko.Computed<string>;
                readonly items: Array<IFilterEditorOperator>;
                readonly displayType: string;
                readonly leftPart: CriteriaOperatorSurface<Criteria.CriteriaOperator>;
                readonly rightPart: any;
                readonly css: string;
                change(type: any, surface: any): void;
                remove(surface: any): void;
                popupService: any;
                canRemove: boolean;
                operatorType: ko.Observable<any>;
                parent: any;
                templateName: string;
                isSelected: ko.Observable<boolean> | ko.Computed<boolean>;
                operatorClass: string;
                helper: FilterEditorHelper;
                reverse: any;
                path: ko.Observable<string> | ko.Computed<string>;
                fieldListProvider: ko.Observable<Utils.IItemsProvider>;
                model: T;
            }
        }
        module Internal {
            class FilterEditorAddOn {
                private _popupService;
                private _action;
                private _updateActions;
                constructor(criteria: Filtering.CriteriaOperatorSurface<Criteria.CriteriaOperator>, popupService: Analytics.Internal.PopupService, action: string, propertyName: any, templateName?: any);
                showPopup: (_: any, element: any) => void;
                popupContentTemplate: string;
                propertyName: string;
                target: Filtering.CriteriaOperatorSurface<Criteria.CriteriaOperator>;
            }
            enum CriteriaSurfaceValidatorState {
                Left = 0,
                Right = 1,
                Unary = 2
            }
            class CriteriaSurfaceValidator {
                customValidate(operator: any, from: CriteriaSurfaceValidatorState): boolean;
                checkLeftPart(leftPart: any): boolean;
                _checkRightPart(criteriaOperator: any): any;
                checkRightPart(rigthPart: any): any;
                aggregateIsValid(criteriaOperator: Criteria.AggregateOperand): any;
                commonOperandValid(criteriaOperator: Criteria.CriteriaOperator): any;
                groupIsValid(criteriaOperator: Criteria.GroupOperator): boolean;
                unaryIsValid(criteriaOperator: Criteria.UnaryOperator): any;
                validateModel(criteriaOperator: Criteria.CriteriaOperator): any;
            }
            class FilterEditorSerializer {
                custom?: (criteriaOperator: Criteria.CriteriaOperator, reverse: boolean) => string;
                serializeGroupOperand(groupOperator: Criteria.GroupOperator, reverse: boolean): any;
                serializeAggregateOperand(aggregateOperand: Criteria.AggregateOperand, reverse: boolean): any;
                serializeOperandProperty(operandProperty: Criteria.OperandProperty): string;
                serializeOperandValue(operandValue: Criteria.OperandValue): any;
                serializeOperandParameter(operandParameter: Criteria.OperandParameter): string;
                serializeBetweenOperator(betweenOperator: Criteria.BetweenOperator, reverse: boolean): any;
                serializeInOperator(inOperator: Criteria.InOperator, reverse: boolean): any;
                serializeBinaryOperator(binaryOperator: Criteria.BinaryOperator, reverse: boolean): any;
                serializeUnaryOperator(unaryOperator: Criteria.UnaryOperator, reverse: boolean): any;
                serializeFunctionOperator(functionOperator: Criteria.FunctionOperator, reverse: boolean): any;
                constructor(operatorTokens?: {
                    "Plus": string;
                    "Minus": string;
                    "Equal": string;
                    "NotEqual": string;
                    "Greater": string;
                    "Less": string;
                    "LessOrEqual": string;
                    "GreaterOrEqual": string;
                }, custom?: (criteriaOperator: Criteria.CriteriaOperator, reverse: boolean) => string);
                serialize(criteriaOperator: Criteria.CriteriaOperator, reverse?: boolean): any;
                deserialize(stringCriteria: string): Criteria.CriteriaOperator;
                deserializeOperand(operand: Criteria.CriteriaOperator): Criteria.CriteriaOperator;
                operatorTokens: any;
            }
            class FilterEditorTreeListController extends TreeListController {
                constructor(selectedItem: ko.Observable<Utils.IDataMemberInfo>);
                itemsFilter(item: Utils.IDataMemberInfo): boolean;
                hasItems(item: Utils.IDataMemberInfo): boolean;
                canSelect(value: TreeListItemViewModel): boolean;
                select(value: TreeListItemViewModel): void;
            }
        }
        module Filtering {
            class AggregateOperandSurface extends CriteriaOperatorSurface<Criteria.AggregateOperand> {
                constructor(operator: Criteria.AggregateOperand, parent: CriteriaOperatorSurface<Criteria.CriteriaOperator>, fieldListProvider: any, path: any);
                readonly leftPart: any;
                readonly rightPart: any;
                dispose(): void;
                contentTemplateName: string;
                property: ko.Observable<any>;
                aggregatedExpression: ko.Observable<any>;
                condition: ko.Observable<any>;
            }
            class BetweenOperandSurface extends CriteriaOperatorSurface<Criteria.BetweenOperator> {
                constructor(operator: Criteria.BetweenOperator, parent: CriteriaOperatorSurface<Criteria.CriteriaOperator>, fieldListProvider: any, path: any);
                readonly leftPart: any;
                readonly rightPart: any[];
                dispose(): void;
                property: ko.Observable<any>;
                end: ko.Observable<any>;
                begin: ko.Observable<any>;
                contentTemplateName: string;
            }
            class BinaryOperandSurface extends CriteriaOperatorSurface<Criteria.BinaryOperator> {
                constructor(operator: Criteria.BinaryOperator, parent: any, fieldListProvider: any, path: any);
                readonly leftPart: CriteriaOperatorSurface<Criteria.CriteriaOperator>;
                readonly rightPart: any;
                dispose(): void;
                contentTemplateName: string;
                leftOperand: ko.Observable<any>;
                rightOperand: ko.Observable<any>;
            }
            class OperandSurfaceBase<T extends Criteria.CriteriaOperator> extends CriteriaOperatorSurface<T> {
                getRealParent(parent: any): any;
                getRealProperty(property: any): any;
                getPropertyName(parent: any, searchProperty: any): Criteria.Utils.IPropertyLocation;
                getConvertableParameters(destinationSpecifics: string): any[];
                constructor(operator: T, parent: CriteriaOperatorSurface<Criteria.CriteriaOperator>, fieldListProvider: any, path: any);
                readonly changeTypeItems: {
                    name: string;
                    instance: any;
                    localizationId: string;
                }[];
                canChange: boolean;
                canRemove: boolean;
                changeValueType: (type: any) => void;
            }
            class FunctionOperandSurface extends OperandSurfaceBase<Criteria.FunctionOperator> {
                constructor(operator: Criteria.FunctionOperator, parent: CriteriaOperatorSurface<Criteria.CriteriaOperator>, fieldListProvider: any, path: any);
                readonly leftPart: CriteriaOperatorSurface<Criteria.CriteriaOperator>;
                readonly rightPart: any[];
                readonly displayType: string;
                dispose(): void;
                canRemove: boolean;
                contentTemplateName: string;
                operands: ko.ObservableArray<any>;
            }
            class GroupOperandSurface extends CriteriaOperatorSurface<Criteria.GroupOperator> {
                constructor(operator: Criteria.GroupOperator, parent: any, fieldListProvider: any, path: any);
                change(type: any, surface: any): void;
                remove(surface: CriteriaOperatorSurface<Criteria.CriteriaOperator>): void;
                create(type: any): void;
                readonly rightPart: CriteriaOperatorSurface<Criteria.CriteriaOperator>[];
                dispose(): void;
                templateName: string;
                operatorClass: string;
                operands: ko.ObservableArray<CriteriaOperatorSurface<Criteria.CriteriaOperator>>;
                createItems: any;
            }
            class InOperandSurface extends CriteriaOperatorSurface<Criteria.InOperator> {
                constructor(operator: Criteria.InOperator, parent: any, fieldListProvider: any, path: any);
                readonly leftPart: any;
                readonly rightPart: any[];
                dispose(): void;
                addValue: () => void;
                contentTemplateName: string;
                operands: ko.ObservableArray<any>;
                criteriaOperator: ko.Observable<any>;
            }
            class OperandParameterSurface extends OperandSurfaceBase<Criteria.OperandParameter> {
                constructor(operator: Criteria.OperandParameter, parent: CriteriaOperatorSurface<Criteria.CriteriaOperator>, fieldListProvider?: any, path?: any);
                changeParameter: (item: Utils.IDataMemberInfo) => void;
                readonly items: any;
                readonly displayType: any;
                operatorClass: string;
                parameterName: ko.Observable<string> | ko.Computed<string>;
                templateName: string;
            }
            class OperandPropertySurface extends OperandSurfaceBase<Criteria.OperandProperty> {
                private _displayName;
                _updateDisplayName(path: any, propertyName: any, displayName: any): void;
                _updateSpecifics(): void;
                constructor(operator: Criteria.OperandProperty, parent: CriteriaOperatorSurface<Criteria.CriteriaOperator>, fieldListProvider?: any, path?: any);
                fieldsOptions: ko.Observable<any>;
                displayName: ko.Computed<string>;
                propertyName: ko.Observable<string>;
                specifics: ko.Observable<string>;
                dataType: ko.Observable<string>;
                readonly items: any;
                readonly displayType: any;
                valueType: ko.Observable<string>;
                changeProperty: (item: Utils.IDataMemberInfo) => void;
                templateName: string;
                operatorClass: string;
            }
            class OperandValueSurface extends OperandSurfaceBase<Criteria.OperandValue> {
                private static _defaultValue;
                private _value;
                private _updateDate;
                readonly items: any[];
                constructor(operator: Criteria.OperandValue, parent: CriteriaOperatorSurface<Criteria.CriteriaOperator>, fieldListProvider: Utils.IItemsProvider, path: any);
                readonly displayType: any;
                changeValue: () => void;
                isDefaultDisplay(): boolean;
                getDefaultValue(): any;
                dataType: ko.Observable<string> | ko.Computed<string>;
                values: ko.Observable<any[]>;
                value: ko.Observable<string> | ko.Computed<string>;
                dataSource: ko.Observable<DataSource> | ko.Computed<DataSource>;
                isEditable: ko.Observable<boolean> | ko.Computed<boolean>;
                templateName: string;
                getNumberEditorOptions: () => any;
            }
            class UnaryOperandSurface extends CriteriaOperatorSurface<Criteria.UnaryOperator> {
                constructor(operator: Criteria.UnaryOperator, parent: any, fieldListProvider?: any, path?: any);
                readonly leftPart: any;
                readonly rightPart: any;
                dispose(): void;
                contentTemplateName: string;
                operand: ko.Observable<any>;
            }
        }
        module Internal {
            function initDisplayText(object: {
                name: string;
                localizationId?: string;
                displayText?: string;
            }): void;
        }
        interface IFilterEditorOperator {
            name: string;
            value: any;
            type: any;
            hidden?: boolean;
            reverse?: boolean;
            localizationId?: string;
            insertVal?: string;
            displayText?: string;
            paramCount?: number;
        }
        class FilterEditorHelper {
            private _initDisplayText;
            constructor(serializer?: any);
            registrateOperator(specific: string, targetEnum: any, value: string, name: string, reverse?: boolean, localizationId?: string): void;
            rtl: boolean;
            parameters: ko.Observable<any[]> | ko.Computed<any[]>;
            canSelectLists: boolean;
            canCreateParameters: boolean;
            canChoiceParameters: boolean;
            canChoiceProperty: boolean;
            serializer: Internal.FilterEditorSerializer;
            criteriaTreeValidator: Internal.CriteriaSurfaceValidator;
            filterEditorOperators: {
                _common: IFilterEditorOperator[];
                string: IFilterEditorOperator[];
                guid: IFilterEditorOperator[];
                integer: IFilterEditorOperator[];
                float: IFilterEditorOperator[];
                date: IFilterEditorOperator[];
                list: IFilterEditorOperator[];
                group: IFilterEditorOperator[];
            };
            onChange: () => void;
            onEditorFocusOut: (criteria: Criteria.CriteriaOperator) => void;
            onSave: (criteria: string) => void;
            onClosing: () => void;
            handlers: {
                create: (criteria: any, popupService: any) => {
                    data: Internal.FilterEditorAddOn;
                    templateName: string;
                };
                change: (criteria: any, popupService: any) => {
                    data: Internal.FilterEditorAddOn;
                    templateName: string;
                };
                changeProperty: (criteria: any, popupService: any) => {
                    data: Internal.FilterEditorAddOn;
                    templateName: string;
                };
                changeValueType: (criteria: any, popupService: any) => {
                    data: Internal.FilterEditorAddOn;
                    templateName: string;
                };
                changeParameter: (criteria: any, popupService: any) => {
                    data: Internal.FilterEditorAddOn;
                    templateName: string;
                };
            };
            generateTreelistOptions(fieldListProvider: any, path: any): any;
            mapper: {
                Aggregate: typeof Filtering.AggregateOperandSurface;
                Property: typeof Filtering.OperandPropertySurface;
                Parameter: typeof Filtering.OperandParameterSurface;
                Value: typeof Filtering.OperandValueSurface;
                Group: typeof Filtering.GroupOperandSurface;
                Between: typeof Filtering.BetweenOperandSurface;
                Binary: typeof Filtering.BinaryOperandSurface;
                Function: typeof Filtering.FunctionOperandSurface;
                In: typeof Filtering.InOperandSurface;
                Unary: typeof Filtering.UnaryOperandSurface;
                Default: typeof Filtering.CriteriaOperatorSurface;
            };
            aceTheme: string;
            getDisplayPropertyName: (path: string, name: string) => JQueryPromise<string>;
        }
        var DefaultFilterEditorHelper: typeof FilterEditorHelper;
        module Internal {
            class FilterEditorCodeCompletor extends CodeCompletor {
                filterEditorAvailable: {
                    operators: Array<{
                        name: string;
                        insertVal: string;
                        paramCount: number;
                    }>;
                    aggregate: Array<{
                        name: string;
                        insertVal: string;
                    }>;
                    functions: Array<{
                        name: string;
                        insertVal: string;
                    }>;
                };
                constructor(options: ICodeCompletorOptions);
                getFunctionsCompletions(): any[];
                getAggregateCompletions(): any[];
                getOperatorCompletions(prefix: any): any[];
            }
        }
        interface IFilterEditorAddon {
            data: Internal.FilterEditorAddOn;
            templateName: string;
        }
        interface IAdvancedState {
            value: ko.Observable<boolean> | ko.Computed<boolean>;
            animated: boolean;
        }
        class FilterEditor extends Utils.Disposable {
            options: ko.Observable<IFilterEditorOptions> | ko.Computed<IFilterEditorOptions>;
            private _displayNameProvider?;
            private _advancedMode;
            private _createMainPopupButtons;
            private _generateOperand;
            private _generateSurface;
            private _validateValue;
            constructor(options: ko.Observable<IFilterEditorOptions> | ko.Computed<IFilterEditorOptions>, fieldListProvider: ko.Observable<Utils.IItemsProvider> | ko.Computed<Utils.IItemsProvider>, rtl?: boolean, _displayNameProvider?: Utils.IDisplayNameProvider);
            change(type: any, surface: any): void;
            readonly helper: FilterEditorHelper;
            readonly path: ko.Observable<string> | ko.Computed<string>;
            displayValue: ko.Observable<string> | ko.Computed<string>;
            modelDisplayValue: ko.Observable<string> | ko.Computed<string>;
            disabled: ko.Observable<boolean> | ko.Computed<boolean>;
            dispose(): void;
            onInput(s: any, e: any): void;
            onFocus(): void;
            onBlur(): void;
            cacheElement($element: JQuery): void;
            updateCriteria(): void;
            onValueChange(value: any): void;
            focusText(): void;
            textFocused: ko.Observable<boolean>;
            aceAvailable: boolean;
            languageHelper: {
                getLanguageMode: () => string;
                createCompleters: (editor: any, bindingContext: any, viewModel: any) => Internal.FilterEditorCodeCompletor[];
            };
            aceOptions: {
                showLineNumbers: boolean;
                showPrintMargin: boolean;
                enableBasicAutocompletion: boolean;
                enableLiveAutocompletion: boolean;
                showGutter: boolean;
            };
            additionalOptions: {
                onChange: (session: any) => void;
                changeTimeout: number;
                onFocus: (_: any) => void;
                onBlur: (_: any) => void;
            };
            editorContainer: ko.Observable<any>;
            textVisible: ko.Observable<boolean>;
            getPopupContainer: (el: any) => JQuery;
            timeout: any;
            animationTimeout: any;
            advancedMode: ko.Computed<boolean>;
            invalidMessage: () => any;
            advancedModeText: () => any;
            modelValueIsValid: ko.Computed<boolean>;
            isSurfaceValid: ko.Computed<boolean>;
            showText: ko.Observable<boolean> | ko.Computed<boolean>;
            displayExpressionConverter: Analytics.Internal.DisplayExpressionConverter;
            isValid: ko.Computed<boolean>;
            fieldListProvider: any;
            createAddButton: (criteria: any) => IFilterEditorAddon;
            createChangeType: (criteria: any) => IFilterEditorAddon;
            createChangeProperty: (criteria: any) => IFilterEditorAddon;
            createChangeParameter: (criteria: any) => IFilterEditorAddon;
            createChangeValueType: (criteria: any) => IFilterEditorAddon;
            operandSurface: ko.Observable<any>;
            operand: any;
            save: () => void;
            popupVisible: ko.Observable<boolean> | ko.Computed<boolean>;
            buttonItems: any[];
            popupService: Analytics.Internal.PopupService;
            value: ko.Observable<string> | ko.Computed<string>;
            rtl: boolean;
        }
        class FilterEditorPlain extends FilterEditor {
            private element;
            private _contentMargins;
            private _topOffset;
            private _defaultActiveTextContentHeightPerc;
            private _defaultActiveTreeContentHeightPerc;
            private _currentActiveTextContentHeightPerc;
            private _currentActiveTreeContentHeightPerc;
            constructor(element: Element, options: ko.Observable<IFilterEditorOptions>, fieldListProvider: ko.Observable<Utils.IItemsProvider>, rtl?: boolean, _displayNameProvider?: Utils.IDisplayNameProvider);
            plainContentHeightPerc: ko.Observable<string>;
            textContentHeightPerc: ko.Observable<any>;
            treeContentHeightPerc: ko.Observable<any>;
        }
        interface IFilterEditorOptions {
            value: ko.Observable<string> | ko.Computed<string>;
            path: ko.Observable<string> | ko.Computed<string>;
            helper?: FilterEditorHelper;
            disabled?: ko.Observable<boolean> | ko.Computed<boolean>;
        }
        class FilterStringOptions implements IFilterEditorOptions {
            private _title;
            constructor(filterString: ko.Observable<string> | ko.Computed<string>, dataMember?: ko.Observable | ko.Computed, disabled?: ko.Observable<boolean> | ko.Computed<boolean>, title?: {
                text: string;
                localizationId?: string;
            });
            popupContainer: string;
            itemsProvider: any;
            disabled: ko.Observable<boolean> | ko.Computed<boolean>;
            resetValue: () => void;
            helper: FilterEditorHelper;
            value: ko.Observable<string> | ko.Computed<string>;
            path: ko.Observable<string> | ko.Computed<string>;
            title: ko.PureComputed<string>;
        }
        module Internal {
            interface IStandardPattern {
                type: string;
                value: any;
                patterns: Array<string>;
            }
            var formatStringStandardPatterns: {
                [key: string]: IStandardPattern;
            };
        }
        interface IPatternItem {
            name: string;
            canRemove: boolean;
        }
        interface IFormatStringEditorActions {
            updatePreview?: (value: string, category: string, pattern: string) => JQueryPromise<string>;
            saveCustomPattern?: (category: string, pattern: string) => JQueryPromise<boolean>;
            removeCustomPattern?: (category: string, pattern: string) => JQueryPromise<boolean>;
        }
        class FormatStringEditor extends Utils.Disposable {
            private _standardPatternSource;
            private _customPatternSource;
            private _lastUpdatePreviewPromise;
            private okAction;
            private _createMainPopupButtons;
            private _convertArray;
            private _scrollToBottom;
            private _updateFormatList;
            private _updateSelection;
            private _updatePreview;
            private _getGeneralPreview;
            private _wrapFormat;
            private _updateCanAddCustomFormat;
            private _initEditor;
            constructor(value: ko.Observable<string>, disabled?: ko.Observable<boolean>, defaultPatterns?: {
                [key: string]: Internal.IStandardPattern;
            }, customPatterns?: {
                [key: string]: Array<string>;
            }, actions?: IFormatStringEditorActions, rtl?: ko.Observable<boolean>, popupContainer?: string);
            updateInputText(propertyName: string, componentInstance: any): void;
            option(name: any, value?: any): any;
            updatePreview(value: string, category: string, pattern: string): JQueryPromise<string>;
            readonly customPatterns: string[];
            readonly isGeneralType: boolean;
            getDisplayText(key: any): any;
            getPopupContainer(el: any): any[];
            currentType: ko.Observable<string>;
            setType: (e: {
                itemData: IPatternItem;
            }) => void;
            setFormat: (e: {
                itemData: IPatternItem;
            }) => void;
            types: Array<IPatternItem>;
            patternList: ko.ObservableArray<IPatternItem>;
            addCustomFormat: () => void;
            removeCustomFormat: (e: any) => void;
            canAddCustomFormat: ko.Observable<boolean>;
            formatPrefix: ko.Observable<string>;
            formatSuffix: ko.Observable<string>;
            previewString: ko.Observable<string>;
            formatResult: ko.Observable<string>;
            selectedFormats: ko.Observable<IPatternItem[]>;
            selectedTypes: ko.Observable<IPatternItem[]>;
            popupService: Analytics.Internal.PopupService;
            popupVisible: ko.Observable<boolean>;
            buttonItems: Array<any>;
            localizationIdMap: {
                [key: string]: Analytics.Internal.ILocalizationInfo;
            };
        }
        module Internal {
            class dxPopupWithAutoHeight extends dxPopup {
                constructor(element: Element, options?: dxPopupOptions);
                _setContentHeight(): void;
            }
        }
    }
    module Internal {
        interface ISearchHighlightOptions {
            text: string | ko.Observable<string>;
            textToSearch: ko.Observable<string> | ko.Computed<string>;
        }
        function cloneHtmlBinding(data: {
            content: any;
        } & Utils.Disposable, element: any, allBindings: any, viewModel: any, bindingContext: any): void;
        class HighlightEngine extends Utils.Disposable {
            private _$spanProtect;
            private _$spanSearch;
            content: ko.Observable<string>;
            private _getHighlightContent;
            constructor(options: ISearchHighlightOptions);
        }
    }
    module Widgets {
        module Internal {
        }
    }
}

declare module DevExpress.Analytics {
    module Utils {
        class ControlsFactory {
            getControlInfo(controlType: string): Elements.IElementMetadata;
            getControlType(model: any): string;
            createControl(model: any, parent: Elements.ElementViewModel, serializer?: IModelSerializer): Elements.IElementViewModel;
            controlsMap: {
                [key: string]: Elements.IElementMetadata;
            };
            registerControl(typeName: string, metadata: Elements.IElementMetadata): void;
            _getPropertyInfo(info: Utils.ISerializationInfoArray, path: string[], position: number): any;
            getPropertyInfo(controlType: string, path: any): any;
        }
        function floatFromModel(val: string): ko.Observable<number>;
        function fromEnum(value: string): ko.Observable<string>;
        function parseBool(val: any): ko.Observable<any>;
        function colorFromString(val: string): ko.Observable<string>;
        function saveAsInt(val: number): string;
        function colorToString(val: string): string;
    }
    module Internal {
        interface IActionsProvider {
            getActions: (context: any) => Utils.IAction[];
        }
        class BaseActionsProvider implements IActionsProvider {
            actions: Utils.IAction[];
            initActions(actions: Utils.IAction[]): void;
            getActions(context: any): Utils.IAction[];
            condition(context: any): boolean;
            setDisabled: (context: any) => void;
        }
        class RequestHelper {
            static generateUri(host: string, uri: string): string;
        }
        class JSDesignerBindingCommon<T> extends Utils.Disposable {
            protected _options: any;
            protected _customEventRaiser?: any;
            sender: T;
            dispose(): void;
            protected _fireEvent(eventName: any, args?: any): void;
            protected _getServerActionUrl(host: any, uri: any): string;
            protected _getAvailableEvents(events: any, prefix?: string): any;
            protected _templateHtml: string;
            protected _getLocalizationRequest(): JQueryPromise<{}>;
            protected _createDisposeFunction(element: HTMLElement): void;
            constructor(_options: any, _customEventRaiser?: any);
        }
    }
    module Elements {
        class Rectangle {
            constructor(left?: number, top?: number, width?: number, height?: number);
            left: ko.Observable<number>;
            top: ko.Observable<number>;
            width: ko.Observable<number>;
            height: ko.Observable<number>;
            className: string;
        }
    }
    module Internal {
        class DragDropHandler extends Utils.Disposable {
            dispose(): void;
            static started: ko.Observable<boolean>;
            surface: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>;
            selection: SurfaceSelection;
            snapHelper: SnapLinesHelper;
            dragHelperContent: DragHelperContent;
            _size: Elements.Size;
            _getAbsoluteSurfacePosition(ui: any): {
                left: number;
                top: number;
            };
            constructor(surface: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, selection: SurfaceSelection, undoEngine: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, snapHelper?: SnapLinesHelper, dragHelperContent?: DragHelperContent);
            addControl(control: any, dropTargetSurface: any, size: any): void;
            recalculateSize(size: any): void;
            helper(draggable: any, event?: any): void;
            startDrag(draggable: any): void;
            drag(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
            stopDrag: (ui: JQueryUI.ResizableUIParams, draggable: any, event?: any) => void;
            doStopDrag(ui: any, draggable: any, event?: any): void;
            cursor: string;
            containment: string;
            alwaysAlt: boolean;
        }
        interface IShortcutActionList {
            processShortcut: (actions: Utils.IAction[], e: JQueryKeyEventObject) => void;
            toolbarItems: Utils.IAction[] | ko.Observable<Utils.IAction[]> | ko.Computed<Utils.IAction[]>;
            enabled?: ko.Observable<boolean> | ko.Computed<boolean>;
        }
        class KeyboardHelper {
            private _selection;
            private _undoEngine;
            constructor(selection: ISelectionProvider, undoEngine?: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>);
            processShortcut(e: JQueryKeyEventObject): boolean;
            processEsc(): void;
            moveSelectedControls(leftUp: boolean, isHoriz: boolean, sign: number): void;
            shortcutMap: {
                [key: number]: (e: any) => boolean;
            };
        }
        class KeyDownHandlersManager {
            private _handlers;
            private _targetElement;
            private readonly _activeHandler;
            private _removeHandler;
            constructor(targetElement: JQuery);
            bindHandler(element: Element, handler: (e: JQueryKeyEventObject) => void): void;
        }
        class DragHelperControlRectangle extends Elements.Rectangle {
            position: number;
            constructor(position: number, left?: number, top?: number, width?: number, height?: number);
        }
        class DragHelperContent extends Elements.Rectangle {
            private _selectionProvider;
            private readonly _isEmpty;
            constructor(selectionProvider: ISelectionProvider);
            reset(): void;
            controls: ko.ObservableArray<Elements.Rectangle | DragHelperControlRectangle>;
            customData: ko.Observable<{}>;
            update(surface: Elements.SurfaceElementBase<any>): void;
            setContent(area: Elements.Rectangle, customData?: {
                template: string;
                data?: any;
            }): void;
            isLocked: ko.Observable<boolean>;
        }
        class SelectionDragDropHandler extends DragDropHandler {
            adjustDropTarget(dropTargetSurface: ISelectionTarget): ISelectionTarget;
            constructor(surface: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, selection: SurfaceSelection, undoEngine: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, snapHelper: SnapLinesHelper, dragHelperContent: DragHelperContent);
            startDrag(control: ISelectionTarget): void;
            drag(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
            getLocation(adjustedTarget: any, item: any): Elements.IArea;
            ajustLocation(adjustedTarget: any, item: any): void;
            doStopDrag(ui: any, _: any): void;
            create(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
        }
        class ToolboxDragDropHandler extends DragDropHandler {
            private _controlsFactory;
            constructor(surface: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, selection: SurfaceSelection, undoEngine: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, snapHelper: SnapLinesHelper, dragHelperContent: DragHelperContent, controlsFactory: Utils.ControlsFactory);
            helper(draggable: any): void;
            doStopDrag(ui: any, draggable: any): void;
        }
    }
    module Widgets {
        class ColorPickerEditor extends Widgets.Editor {
            constructor(info: Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
            displayValue: ko.Computed<string>;
        }
        class FieldListEditor extends Widgets.Editor {
            constructor(modelPropertyInfo: any, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
            path: ko.PureComputed<any>;
            treeListController: Internal.TreeListController;
        }
        module Internal {
            class DataMemberTreeListController implements ITreeListController {
                dispose(): void;
                itemsFilter(item: Utils.IDataMemberInfo): boolean;
                hasItems(item: Utils.IDataMemberInfo): boolean;
                canSelect(value: Widgets.Internal.TreeListItemViewModel): boolean;
                select(value: Widgets.Internal.TreeListItemViewModel): void;
                selectedItem: any;
                suppressActions: boolean;
            }
        }
        class DataMemberEditor extends FieldListEditor {
            constructor(modelPropertyInfo: any, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
            treeListController: Internal.DataMemberTreeListController;
        }
        module Internal {
            class RequiredNullableEditor extends Editor {
                _getEditorValidationRules(): any[];
            }
            function createNumericEditor(dotNetTypeFullName: string, specifics: string): {
                header: string;
                editorType: any;
            };
        }
        var coreEditorTemplates: {
            guid: {
                header: string;
                editorType: typeof GuidEditor;
            };
            date: {
                header: string;
                editorType: typeof Internal.RequiredNullableEditor;
            };
            borders: {
                header: string;
            };
            objecteditorCustom: {
                custom: string;
                editorType: typeof PropertyGridEditor;
            };
            field: {
                header: string;
                editorType: typeof FieldListEditor;
            };
            dataMember: {
                header: string;
                editorType: typeof DataMemberEditor;
            };
            filterEditor: {
                header: string;
            };
            formatEditor: {
                header: string;
            };
            expressionEditor: {
                header: string;
            };
            customColorEditor: {
                header: string;
                editorType: typeof ColorPickerEditor;
            };
            sbyte: {
                header: string;
                editorType: any;
            };
            decimal: {
                header: string;
                editorType: any;
            };
            int64: {
                header: string;
                editorType: any;
            };
            int32: {
                header: string;
                editorType: any;
            };
            int16: {
                header: string;
                editorType: any;
            };
            single: {
                header: string;
                editorType: any;
            };
            double: {
                header: string;
                editorType: any;
            };
            byte: {
                header: string;
                editorType: any;
            };
            uint16: {
                header: string;
                editorType: any;
            };
            uint32: {
                header: string;
                editorType: any;
            };
            uint64: {
                header: string;
                editorType: any;
            };
        };
        var editorTemplates: any;
    }
    module Elements {
        interface IArea {
            top?: number;
            left?: number;
            right?: number;
            bottom?: number;
            width?: number;
            height?: number;
        }
        interface ISurfaceContext {
            measureUnit: ko.Observable<string> | ko.Computed<string>;
            pageWidth?: ko.Observable<number> | ko.Computed<number>;
            pageHeight?: ko.Observable<number> | ko.Computed<number>;
            snapGridSize?: ko.Observable<number> | ko.Computed<number>;
            margins?: IMargins;
            zoom?: ko.Observable<number> | ko.Computed<number>;
            dpi?: ko.Observable<number> | ko.Computed<number>;
            isFit?: (dropTarget: Internal.ISelectionTarget) => boolean;
            rtl?: ko.Observable<boolean> | ko.Computed<boolean>;
        }
        class SurfaceElementArea<M extends ElementViewModel> extends Utils.Disposable {
            _control: M;
            _width: ko.Observable<number> | ko.Computed<number>;
            _height: ko.Observable<number> | ko.Computed<number>;
            _x: ko.Observable<number> | ko.Computed<number>;
            _y: ko.Observable<number> | ko.Computed<number>;
            _context: ISurfaceContext;
            _createSurface: (item: ElementViewModel) => any;
            private _container;
            private _getX;
            private _setX;
            getRoot(): ISurfaceContext;
            preInitProperties(control: M, context: ISurfaceContext, unitProperties: Internal.IUnitProperties<M>): void;
            constructor(control: M, context: ISurfaceContext, unitProperties: Internal.IUnitProperties<M>);
            rect: ko.Observable<IArea> | ko.Computed<IArea>;
            container(): SurfaceElementArea<ElementViewModel>;
            beforeRectUpdated(rect: any): any;
            rtlLayout(): boolean;
            getControlModel(): M;
        }
        class SurfaceElementBase<M extends ElementViewModel> extends SurfaceElementArea<M> implements Internal.ISelectionTarget {
            private _countSelectedChildren;
            context: ISurfaceContext;
            constructor(control: M, context: ISurfaceContext, unitProperties: Internal.IUnitProperties<M>);
            focused: ko.Observable<boolean> | ko.Computed<boolean>;
            selected: ko.Observable<boolean> | ko.Computed<boolean>;
            isSelected: ko.Observable<boolean> | ko.Computed<boolean>;
            cssCalculator: Internal.CssCalculator;
            underCursor: ko.Observable<Internal.IHoverInfo> | ko.Computed<Internal.IHoverInfo>;
            readonly parent: any;
            checkParent(surfaceParent: Internal.ISelectionTarget): boolean;
            allowMultiselect: boolean;
            css: ko.Observable<any> | ko.Computed<any>;
            contentCss: ko.Observable<any> | ko.Computed<any>;
            _getChildrenHolderName(): string;
            getChildrenCollection(): ko.ObservableArray<any>;
            absolutePosition: Point;
            updateAbsolutePosition(): void;
            canDrop(): boolean;
            afterUpdateAbsolutePosition(): void;
            absoluteRect: ko.Computed<IArea>;
            getUsefulRect: () => IArea;
            locked: boolean;
        }
        interface IElementMetadata {
            info: Utils.ISerializationInfoArray;
            surfaceType: any;
            type?: any;
            nonToolboxItem?: boolean;
            isToolboxItem?: boolean;
            toolboxIndex?: number;
            defaultVal?: {};
            size?: string;
            isContainer?: boolean;
            isCopyDeny?: boolean;
            isPasteDeny?: boolean;
            isDeleteDeny?: boolean;
            popularProperties?: string[];
            canDrop?: (dropTarget: Internal.ISelectionTarget, dragFrom?: ElementViewModel) => boolean;
            elementActionsTypes?: any;
            parentType?: string;
            displayName?: string;
        }
        interface IElementViewModel {
            controlType: string;
            name: ko.Observable<string> | ko.Computed<string>;
            parentModel: ko.Observable<IElementViewModel>;
            addChild: (element: IElementViewModel) => void;
            addChilds: (array: IElementViewModel[]) => void;
            removeChild: (element: IElementViewModel) => void;
            removeChilds: (array: IElementViewModel[]) => void;
            getNearestParent: (dropTarget: IElementViewModel) => IElementViewModel;
        }
        interface IControlPropertiesViewModel {
            isPropertyDisabled: (name: string) => boolean;
            isPropertyVisible: (name: string) => boolean;
            isPropertyModified: (name: string) => boolean;
            controlType?: string;
            actions: Internal.IModelAction[];
        }
        class ElementViewModel extends Utils.Disposable implements IElementViewModel, IControlPropertiesViewModel {
            getPropertyDefaultValue(propertyName: string): any;
            getPropertyInfo(propertyName: string): Utils.ISerializationInfo;
            getInfo(): Utils.ISerializationInfoArray;
            createControl(model: any, serializer?: Utils.IModelSerializer): IElementViewModel;
            dispose(): void;
            preInitProperties(model: any, parent: ElementViewModel, serializer?: Utils.IModelSerializer): void;
            constructor(model: any, parent: ElementViewModel, serializer?: Utils.IModelSerializer);
            getNearestParent(target: IElementViewModel): any;
            getControlInfo(): IElementMetadata;
            getMetaData(): {
                isContainer: boolean;
                isCopyDeny: boolean;
                isDeleteDeny: boolean;
                canDrop: (dropTarget: Internal.ISelectionTarget, dragFrom?: ElementViewModel) => boolean;
                isPasteDeny: boolean;
            };
            _hasModifiedValue(name: any): any;
            name: ko.Observable<string> | ko.Computed<string>;
            controlType: string;
            createChild(info: {}): ElementViewModel;
            removeChilds(controls: ElementViewModel[]): void;
            addChilds(controls: ElementViewModel[]): void;
            removeChild(control: ElementViewModel): void;
            addChild(control: IElementViewModel): void;
            isPropertyVisible(name: string): boolean;
            isPropertyDisabled(name: string): boolean;
            isPropertyModified(name: string): any;
            getControlFactory(): Utils.ControlsFactory;
            resetValue: (propertyName: string) => void;
            isResettableProperty(propertyName: string): boolean;
            surface: any;
            parentModel: ko.Observable<ElementViewModel>;
            readonly root: ElementViewModel;
            rtl(): boolean;
            actions: Internal.IModelAction[];
            update: ko.Observable<boolean>;
        }
        interface IMargins {
            bottom: ko.Observable<number> | ko.Computed<number>;
            left: ko.Observable<number> | ko.Computed<number>;
            right: ko.Observable<number> | ko.Computed<number>;
            top: ko.Observable<number> | ko.Computed<number>;
        }
        class Margins implements IMargins {
            static defaultVal: string;
            static unitProperties: string[];
            getInfo(): Utils.ISerializationInfo[];
            constructor(left: any, right: any, top: any, bottom: number);
            isEmpty(): boolean;
            static fromString(value?: string): Margins;
            toString(): string;
            bottom: ko.Observable<number> | ko.Computed<number>;
            left: ko.Observable<number> | ko.Computed<number>;
            right: ko.Observable<number> | ko.Computed<number>;
            top: ko.Observable<number> | ko.Computed<number>;
        }
        module Metadata {
            var left: Utils.ISerializationInfo, right: Utils.ISerializationInfo, top: Utils.ISerializationInfo, bottom: Utils.ISerializationInfo, all: Utils.ISerializationInfo;
            var paddingSerializationsInfo: Utils.ISerializationInfo[];
        }
        class PaddingModel extends Utils.Disposable {
            left: ko.Observable<number> | ko.Computed<number>;
            right: ko.Observable<number> | ko.Computed<number>;
            top: ko.Observable<number> | ko.Computed<number>;
            bottom: ko.Observable<number> | ko.Computed<number>;
            dpi: ko.Observable<number> | ko.Computed<number>;
            static defaultVal: string;
            static unitProperties: string[];
            getInfo(): Utils.ISerializationInfo[];
            resetValue(): void;
            isEmpty(): boolean;
            applyFromString(value: string): this;
            static from(val: any): PaddingModel;
            toString(): string;
            _toString(inner?: boolean): string;
            constructor(left?: ko.Observable<number> | ko.Computed<number>, right?: ko.Observable<number> | ko.Computed<number>, top?: ko.Observable<number> | ko.Computed<number>, bottom?: ko.Observable<number> | ko.Computed<number>, dpi?: ko.Observable<number> | ko.Computed<number>);
            all: ko.Computed<number>;
        }
        interface IPoint {
            x: ko.Observable<number> | ko.Computed<number>;
            y: ko.Observable<number> | ko.Computed<number>;
        }
        class Point implements IPoint {
            static unitProperties: string[];
            constructor(x: any, y: number);
            getInfo(): Utils.ISerializationInfoArray;
            static fromString(value?: string): Point;
            toString(): string;
            x: ko.Observable<number> | ko.Computed<number>;
            y: ko.Observable<number> | ko.Computed<number>;
        }
        class SerializableModel extends Utils.Disposable {
            preInitProperties(model: any, serializer?: Utils.IModelSerializer, info?: Utils.ISerializationInfoArray): void;
            constructor(model: any, serializer?: Utils.IModelSerializer, info?: Utils.ISerializationInfoArray);
            getInfo: () => Utils.ISerializationInfoArray;
        }
        interface ISize {
            width: ko.Observable<number> | ko.Computed<number>;
            height: ko.Observable<number> | ko.Computed<number>;
            isPropertyDisabled: (name: string) => void;
        }
        class Size implements ISize {
            static unitProperties: string[];
            constructor(width: any, height: number);
            getInfo(): Utils.ISerializationInfoArray;
            static fromString(value?: string): Size;
            toString(): string;
            isPropertyDisabled: (name: string) => any;
            isPropertyVisible: (name: string) => boolean;
            width: ko.Observable<number> | ko.Computed<number>;
            height: ko.Observable<number> | ko.Computed<number>;
        }
    }
    module Internal {
        function getToolboxItems(controlsMap: {
            [key: string]: Elements.IElementMetadata;
        }): Utils.ToolboxItem[];
        function blur(element: Element): void;
        function guid(): string;
    }
    module Internal {
        class AjaxSetup {
            ajaxSettings: JQueryAjaxSettings;
            sendRequest(settings: JQueryAjaxSettings): JQueryXHR;
        }
    }
    module Utils {
        var ajaxSetup: Internal.AjaxSetup;
    }
    module Internal {
        interface IDisplayedObject {
            name: ko.Observable<string> | ko.Computed<string>;
        }
        interface IDesignControlsHelper extends Utils.IDisposable {
            getControls: (target: any) => ko.ObservableArray<IDisplayedObject>;
            allControls: ko.ObservableArray<IDisplayedObject>;
            getNameProperty?: (model: any) => ko.Observable<string> | ko.Computed<string>;
        }
        class DesignControlsHelper extends Utils.Disposable implements IDesignControlsHelper {
            protected target: any;
            private collectionNames?;
            private _handlers;
            private _setText;
            private _visitedCollections;
            private _subscriptions;
            getNameProperty(model: any): any;
            protected _setName(value: any): void;
            dispose(): void;
            private added;
            private deleted;
            _collectControls(target: any): void;
            constructor(target: any, handlers?: Array<{
                added: (control: any) => void;
                deleted?: (control: any) => void;
            }>, collectionNames?: string[]);
            getControls(target: any): ko.ObservableArray<IDisplayedObject>;
            allControls: ko.ObservableArray<IDisplayedObject>;
        }
        interface IStyleContainer {
            rtl: () => boolean;
        }
        function patchPositionByRTL(position: string, rtl: boolean): string;
        class CssCalculator {
            private _rtlLayout;
            static DEFAULT_BORDER: string;
            private _control;
            private _getPixelValueFromUnit;
            private _patchPosition;
            private _getBorderWidth;
            createBorder(dashStyle: any, width: any, color: any, positions: any, position: any): {};
            createControlBorder(borderStyle: any, width: any, color: any, positions: any, position: any, defaultColor?: string): {};
            createBorders(borderStyle: any, width: any, color: any, positions: any, defaultColor?: string): any;
            createZipCodeFont(height: any): {};
            createFont(fontString: any): {};
            createVerticalAlignment(alignment: string): {};
            createHorizontalAlignment(alignment: string): {};
            createStrokeDashArray(style: any, width: any): string;
            createWordWrap(wordwrap: boolean, multiline: boolean): {};
            createAngle(angle: any): {
                '-webkit-transform': string;
                '-moz-transform': string;
                '-o-transform': string;
                '-ms-transform': string;
                'transform': string;
            };
            constructor(control: IStyleContainer, _rtlLayout: ko.Observable<boolean> | ko.Computed<boolean>);
            borderCss: any;
            fontCss: any;
            zipCodeFontCss: any;
            textAlignmentCss: any;
            foreColorCss: any;
            paddingsCss: any;
            backGroundCss: any;
            stroke: any;
            strokeWidth: any;
            strokeWidthWithWidth: any;
            strokeDashArray: any;
            strokeDashArrayWithWidth: any;
            crossBandBorder: any;
            angle: any;
            wordWrapCss: any;
            cellBorder: any;
            zipCodeAlignment: any;
            contentSizeCss(controlSurfaceWidth: number, controlSurfaceHeight: number, zoom?: number, borders?: string, paddings?: Elements.PaddingModel): {
                top: number;
                left: number;
                right: number;
                bottom: number;
                width: number;
                height: number;
            };
        }
        var editorTypeMapper: {
            "Enum": any;
            "System.String": any;
            "System.Guid": {
                header: string;
                editorType: typeof Widgets.GuidEditor;
            };
            "System.SByte": {
                header: string;
                editorType: any;
            };
            "System.Decimal": {
                header: string;
                editorType: any;
            };
            "System.Int64": {
                header: string;
                editorType: any;
            };
            "System.Int32": {
                header: string;
                editorType: any;
            };
            "System.Int16": {
                header: string;
                editorType: any;
            };
            "System.Single": {
                header: string;
                editorType: any;
            };
            "System.Double": {
                header: string;
                editorType: any;
            };
            "System.Byte": {
                header: string;
                editorType: any;
            };
            "System.UInt16": {
                header: string;
                editorType: any;
            };
            "System.UInt32": {
                header: string;
                editorType: any;
            };
            "System.UInt64": {
                header: string;
                editorType: any;
            };
            "System.Boolean": any;
            "System.DateTime": any;
            "DevExpress.DataAccess.Expression": {
                header: string;
            };
        };
        function getEditorType(typeString: string): {
            header?: any;
            content?: any;
            custom?: any;
        };
        function getTypeNameFromFullName(controlType: string): string;
        function getShortTypeName(controlType: string): string;
        function getControlFullName(value: any): string;
        function getImageClassName(controlType: string, isTemplate?: boolean): string;
        function getUniqueNameForNamedObjectsArray(objects: any[], prefix: string, names?: string[]): string;
        function getUniqueName(names: string[], prefix: string): string;
        interface ILocalizationSettings extends IGlobalizeSettings {
            localization?: {
                [stringId: string]: string;
            };
        }
        interface IGlobalizeSettings {
            currentCulture?: string;
            cldrData?: string;
            cldrSupplemental?: string;
        }
        function initGlobalize(settings: IGlobalizeSettings): void;
        interface IHoverInfo {
            isOver: boolean;
            x: number;
            y: number;
            offsetX?: number;
            offsetY?: number;
            isNotDropTarget?: boolean;
        }
        class HoverInfo implements IHoverInfo {
            private _x;
            private _y;
            isOver: boolean;
            x: number;
            y: number;
        }
        function processTextEditorHotKeys(event: JQueryKeyEventObject, delegates: any): void;
        class InlineTextEdit extends Utils.Disposable {
            private _showInline;
            text: ko.Observable<string> | ko.Computed<string>;
            visible: ko.Observable<boolean> | ko.Computed<boolean>;
            keypressAction: any;
            show: any;
            constructor(selection: ISelectionProvider);
        }
        class ObjectStructureTreeListController implements Widgets.Internal.ITreeListController {
            dispose(): void;
            constructor(propertyNames?: string[], listPropertyNames?: string[]);
            canSelect(value: Widgets.Internal.TreeListItemViewModel): boolean;
            dragDropHandler: DragDropHandler;
            selectedItem: any;
            dblClickHandler: (item: Widgets.Internal.TreeListItemViewModel) => void;
            select: (value: Widgets.Internal.TreeListItemViewModel) => void;
            itemsFilter: (item: Utils.IDataMemberInfo) => boolean;
            hasItems: (item: Utils.IDataMemberInfo) => boolean;
            getActions: (item: Widgets.Internal.TreeListItemViewModel) => Utils.IAction[];
            showIconsForChildItems: (item?: Widgets.Internal.TreeListItemViewModel) => boolean;
        }
        interface IRootItem {
            model: any;
            displayName?: string;
            name: string;
            className: string;
            data?: any;
        }
        class ObjectStructureProviderBase extends Utils.Disposable implements Utils.IItemsProvider {
            getClassName(instance: any): any;
            createItem(currentTarget: any, propertyName: string, propertyValue: any, result: Utils.IDataMemberInfo[]): void;
            getMemberByPath(target: any, path: string): any;
            getObjectPropertiesForPath(target: any, path: string, propertyName?: string): Utils.IDataMemberInfo[];
            createArrayItem(currentTarget: Array<any>, result: Utils.IDataMemberInfo[], propertyName?: any): void;
            getItems: (pathRequest: Utils.IPathRequest) => JQueryPromise<Utils.IDataMemberInfo[]>;
            selectedPath: ko.Observable<string> | ko.Computed<string>;
            selectedMember: ko.Observable | ko.Computed;
        }
        class ObjectExplorerProvider extends ObjectStructureProviderBase {
            getPathByMember: (model: any) => string;
            createArrayItem(currentTarget: Array<any>, result: Utils.IDataMemberInfo[], propertyName?: any): void;
            createItem(currentTarget: any, propertyName: string, propertyValue: any, result: Utils.IDataMemberInfo[]): void;
            constructor(rootITems: IRootItem[], listPropertyNames?: string[], member?: ko.Observable | ko.Computed, getPathByMember?: any);
            path: ko.Observable<string> | ko.Computed<string>;
            listPropertyNames: string[];
        }
        class ObjectStructureProvider extends ObjectStructureProviderBase {
            constructor(target: any, displayName?: string, localizationId?: string);
        }
        var papperKindMapper: {
            A2: {
                width: number;
                height: number;
            };
            A3: {
                width: number;
                height: number;
            };
            A3Extra: {
                width: number;
                height: number;
            };
            A3ExtraTransverse: {
                width: number;
                height: number;
            };
            A3Rotated: {
                width: number;
                height: number;
            };
            A3Transverse: {
                width: number;
                height: number;
            };
            A4: {
                width: number;
                height: number;
            };
            A4Extra: {
                width: number;
                height: number;
            };
            A4Plus: {
                width: number;
                height: number;
            };
            A4Rotated: {
                width: number;
                height: number;
            };
            A4Small: {
                width: number;
                height: number;
            };
            A4Transverse: {
                width: number;
                height: number;
            };
            A5: {
                width: number;
                height: number;
            };
            A5Extra: {
                width: number;
                height: number;
            };
            A5Rotated: {
                width: number;
                height: number;
            };
            A5Transverse: {
                width: number;
                height: number;
            };
            A6: {
                width: number;
                height: number;
            };
            A6Rotated: {
                width: number;
                height: number;
            };
            APlus: {
                width: number;
                height: number;
            };
            B4: {
                width: number;
                height: number;
            };
            B4Envelope: {
                width: number;
                height: number;
            };
            B4JisRotated: {
                width: number;
                height: number;
            };
            B5: {
                width: number;
                height: number;
            };
            B5Envelope: {
                width: number;
                height: number;
            };
            B5Extra: {
                width: number;
                height: number;
            };
            B5JisRotated: {
                width: number;
                height: number;
            };
            B5Transverse: {
                width: number;
                height: number;
            };
            B6Envelope: {
                width: number;
                height: number;
            };
            B6Jis: {
                width: number;
                height: number;
            };
            B6JisRotated: {
                width: number;
                height: number;
            };
            BPlus: {
                width: number;
                height: number;
            };
            C3Envelope: {
                width: number;
                height: number;
            };
            C4Envelope: {
                width: number;
                height: number;
            };
            C5Envelope: {
                width: number;
                height: number;
            };
            C65Envelope: {
                width: number;
                height: number;
            };
            C6Envelope: {
                width: number;
                height: number;
            };
            CSheet: {
                width: number;
                height: number;
            };
            DLEnvelope: {
                width: number;
                height: number;
            };
            DSheet: {
                width: number;
                height: number;
            };
            ESheet: {
                width: number;
                height: number;
            };
            Executive: {
                width: number;
                height: number;
            };
            Folio: {
                width: number;
                height: number;
            };
            GermanLegalFanfold: {
                width: number;
                height: number;
            };
            GermanStandardFanfold: {
                width: number;
                height: number;
            };
            InviteEnvelope: {
                width: number;
                height: number;
            };
            IsoB4: {
                width: number;
                height: number;
            };
            ItalyEnvelope: {
                width: number;
                height: number;
            };
            JapaneseDoublePostcard: {
                width: number;
                height: number;
            };
            JapaneseDoublePostcardRotated: {
                width: number;
                height: number;
            };
            JapanesePostcard: {
                width: number;
                height: number;
            };
            Ledger: {
                width: number;
                height: number;
            };
            Legal: {
                width: number;
                height: number;
            };
            LegalExtra: {
                width: number;
                height: number;
            };
            Letter: {
                width: number;
                height: number;
            };
            LetterExtra: {
                width: number;
                height: number;
            };
            LetterExtraTransverse: {
                width: number;
                height: number;
            };
            LetterPlus: {
                width: number;
                height: number;
            };
            LetterRotated: {
                width: number;
                height: number;
            };
            LetterSmall: {
                width: number;
                height: number;
            };
            LetterTransverse: {
                width: number;
                height: number;
            };
            MonarchEnvelope: {
                width: number;
                height: number;
            };
            Note: {
                width: number;
                height: number;
            };
            Number10Envelope: {
                width: number;
                height: number;
            };
            Number11Envelope: {
                width: number;
                height: number;
            };
            Number12Envelope: {
                width: number;
                height: number;
            };
            Number14Envelope: {
                width: number;
                height: number;
            };
            Number9Envelope: {
                width: number;
                height: number;
            };
            PersonalEnvelope: {
                width: number;
                height: number;
            };
            Prc16K: {
                width: number;
                height: number;
            };
            Prc16KRotated: {
                width: number;
                height: number;
            };
            Prc32K: {
                width: number;
                height: number;
            };
            Prc32KBig: {
                width: number;
                height: number;
            };
            Prc32KBigRotated: {
                width: number;
                height: number;
            };
            Prc32KRotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber1: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber10: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber10Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber1Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber2: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber2Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber3: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber3Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber4: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber4Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber5: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber5Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber6: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber6Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber7: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber7Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber8: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber8Rotated: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber9: {
                width: number;
                height: number;
            };
            PrcEnvelopeNumber9Rotated: {
                width: number;
                height: number;
            };
            Quarto: {
                width: number;
                height: number;
            };
            Standard10x11: {
                width: number;
                height: number;
            };
            Standard10x14: {
                width: number;
                height: number;
            };
            Standard11x17: {
                width: number;
                height: number;
            };
            Standard12x11: {
                width: number;
                height: number;
            };
            Standard15x11: {
                width: number;
                height: number;
            };
            Standard9x11: {
                width: number;
                height: number;
            };
            Statement: {
                width: number;
                height: number;
            };
            Tabloid: {
                width: number;
                height: number;
            };
            TabloidExtra: {
                width: number;
                height: number;
            };
            USStandardFanfold: {
                width: number;
                height: number;
            };
        };
        var _addErrorPrefix: boolean;
        function _processError(errorThrown: string, deferred: JQueryDeferred<any>, jqXHR: any, textStatus: any, processErrorCallback?: (message: string, jqXHR: any, textStatus: any) => void): void;
        var _errorProcessor: {
            handlers: any[];
            call: (e: any) => void;
        };
        function processErrorEvent(func: any): Utils.IDisposable;
        interface IResizableOptions extends JQueryUI.ResizableOptions {
            starting?: () => void;
            $element?: Element;
            stopped?: () => void;
            zoom?: number;
            minimumWidth?: ko.Observable<number> | number;
            maximumWidth?: ko.Observable<number> | number;
        }
        class CustomSortedArrayStore extends CustomStore {
            static _sortItems(items: any[], sortPropertyName: string): any[];
            static _createOptions(items: any, sortPropertyName: any): {
                load: (options: any) => JQueryPromise<{}>;
                byKey: (key: any) => any;
            };
            constructor(items: any[], sortPropertyName?: string);
        }
        class SortedArrayStore extends ArrayStore {
            constructor(options: any, sortPropertyName?: string);
        }
        class ControlsStore extends Utils.Disposable {
            private _filter;
            dataSource: ko.Computed<DataSource>;
            constructor(allControls: ko.ObservableArray<any>);
            setFilter(filter: any): void;
            resetFilter(): void;
            visible: ko.Computed<boolean>;
        }
        function findSurface(viewModel: Elements.IElementViewModel): ISelectionTarget;
        function getControlNewAbsolutePositionOnResize(snapHelper: SnapLinesHelper, absolutePosition: {
            top: number;
            left: number;
        }, ui: {
            originalSize: {
                width: number;
                height: number;
            };
            size: {
                width: number;
                height: number;
            };
        }, delta: {
            x: number;
            y: number;
            width: number;
            height: number;
        }): {
            top: number;
            left: number;
            bottom: number;
            right: number;
        };
        function getControlRect(element: JQuery, control: ISelectionTarget, surface: Elements.ISurfaceContext): {
            top: number;
            left: number;
            width: number;
            height: number;
        };
        function minHeightWithoutScroll(element: HTMLElement): number;
        function chooseBetterPositionOf(html: any, designer: any): any;
        function updateSurfaceContentSize(surfaceSize: ko.Observable<number> | ko.Computed<number>, root: Element, rtl?: boolean): () => void;
        function validateName(nameCandidate: any): boolean;
        function replaceInvalidSymbols(text: string): string;
        var nameValidationRules: {
            type: string;
            validationCallback: (options: any) => boolean;
            message: any;
        }[];
        interface ICombinedProperty {
            result: any;
            subscriptions: ko.Subscription[];
        }
        class CombinedObject {
            private static getInfo;
            private static isPropertyDisabled;
            private static isPropertyVisible;
            private static mergeProperty;
            static _createProperty(result: any, propertyName: any, propertyValue: any): void;
            static _merge(controls: any[], undoEngine?: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, customMerge?: any, ignoreProperties?: any): {
                result: {};
                subscriptions: any[];
            };
            static mergeControls(controls: any[], undoEngine?: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, customMerge?: any, ignoreProperties?: string[]): {
                result: any;
                subscriptions: any[];
            };
            static getEditableObject(selectionProvider: ISelectionProvider, undoEngine?: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, customMerge?: any): ko.Observable | ko.Computed;
        }
        interface ISelectingEvent {
            control: ISelectionTarget;
            cancel: boolean;
            ctrlKey?: boolean;
        }
        interface ISelectionTarget {
            rect: ko.Observable<Elements.IArea> | ko.Computed<Elements.IArea>;
            focused: ko.Observable<boolean> | ko.Computed<boolean>;
            selected: ko.Observable<boolean> | ko.Computed<boolean>;
            underCursor: ko.Observable<IHoverInfo> | ko.Computed<IHoverInfo>;
            allowMultiselect: boolean;
            locked: boolean;
            canDrop: () => boolean;
            getControlModel: () => Elements.ElementViewModel;
            checkParent: (surfaceParent: ISelectionTarget) => boolean;
            parent: ISelectionTarget;
            getChildrenCollection: () => ko.ObservableArray<ISelectionTarget>;
        }
        interface ISelectionProvider extends Utils.IDisposable {
            focused: ko.Observable<ISelectionTarget> | ko.Computed<ISelectionTarget>;
            selectedItems: ISelectionTarget[];
            initialize(surface?: ISelectionTarget): any;
            selecting(event: ISelectingEvent): any;
            unselecting(surface: ISelectionTarget): any;
            swapFocusedItem(surface: ISelectionTarget): any;
            ignoreMultiSelectProperties?: string[];
        }
        class SurfaceSelection extends Utils.Disposable implements ISelectionProvider {
            ignoreMultiSelectProperties: string[];
            dispose(): void;
            private _focused;
            private _firstSelected;
            private _selectedControls;
            private _selectedControlsInner;
            private _removeFromSelection;
            private _setFocused;
            private _resetTabPanelFocus;
            constructor(ignoreMultiSelectProperties?: string[]);
            focused: ko.PureComputed<ISelectionTarget>;
            readonly selectedItems: ISelectionTarget[];
            clear(): void;
            reset(): void;
            applySelection(): void;
            selectItems(items: any): void;
            updateSelection(control: ISelectionTarget): void;
            swapFocusedItem(control: ISelectionTarget): void;
            initialize(control?: ISelectionTarget): void;
            clickHandler(control?: ISelectionTarget, event?: {
                ctrlKey: boolean;
            }): void;
            selecting(event: ISelectingEvent): void;
            unselecting(control: ISelectionTarget): void;
            selectionWithCtrl(control: ISelectionTarget): void;
            dropTarget: ISelectionTarget;
            expectClick: boolean;
            disabled: ko.Observable<boolean>;
        }
        function deleteSelection(selection: ISelectionProvider): void;
        function findNextSelection(removedElement: ISelectionTarget): ISelectionTarget;
        class SnapLinesCollector {
            private _verticalSnapLines;
            private _horizontalSnapLines;
            private _snapTargetToIgnore;
            private _appendSnapLine;
            private _collectSnaplines;
            _getCollection(parent: any): {
                rect: ko.Observable<Elements.IArea>;
            }[];
            _enumerateCollection(parent: any, parentAbsoluteProsition: {
                top: number;
                left: number;
            }, callback: (item: any, itemAbsoluteRect: {
                left: number;
                right: number;
                top: number;
                bottom: number;
            }) => void): void;
            collectSnaplines(root: any, snapTargetToIgnore: any): {
                vertical: ISnapLine[];
                horizontal: ISnapLine[];
            };
        }
        class SnapLinesHelper {
            static snapTolerance: number;
            private _snapTolerance;
            private _surfaceContext;
            private _snapLinesCollector;
            private _findClosestSnapLine;
            _getActiveSnapLines(position1: number, position2: number, snapLines: ISnapLine[]): {
                lines: any[];
                distance: number;
            };
            constructor(surface?: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, snapTolerance?: number, snapLinesCollector?: SnapLinesCollector);
            updateSnapLines(snapTargetToIgnore?: any): void;
            deactivateSnapLines(): void;
            activateSnapLines(position: {
                left: number;
                top: number;
                right: number;
                bottom: number;
            }): {
                left: number;
                top: number;
            };
            snapPosition(position: number, horizontal: boolean): number;
            snapLineSurfaces: SnapLineSurface[];
            verticalSnapLines: ISnapLine[];
            horizontalSnapLines: ISnapLine[];
        }
        interface ISnapLine {
            position: number;
            limitInf: number;
            limSup: number;
        }
        class SnapLineSurface {
            private static _blankPosition;
            private _position;
            transform(): string;
            updatePosition(position: {
                top: number;
                left: number;
                width: number;
                height: number;
            }): void;
            reset(): void;
        }
    }
    module Tools {
        var ActionId: {
            Cut: string;
            Copy: string;
            Paste: string;
            Delete: string;
            Undo: string;
            Redo: string;
            ZoomOut: string;
            ZoomSelector: string;
            ZoomIn: string;
        };
    }
    module Internal {
        class ActionListsBase extends Utils.Disposable implements IShortcutActionList {
            constructor(enabled?: ko.Observable<boolean> | ko.Computed<boolean>);
            processShortcut(actions: Utils.IAction[], e: JQueryKeyEventObject): void;
            shouldIgnoreProcessing(e: JQueryKeyEventObject): boolean;
            enabled: ko.Observable<boolean> | ko.Computed<boolean>;
            toolbarItems: Utils.IAction[] | ko.Observable<Utils.IAction[]> | ko.Computed<Utils.IAction[]>;
        }
        class ActionLists extends ActionListsBase {
            _registerAction(container: Array<Utils.IAction>, action: Utils.IAction): void;
            private _keyboardHelper;
            constructor(surfaceContext: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, selection: ISelectionProvider, undoEngine: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, customizeActions?: (actions: Utils.IAction[]) => void, enabled?: ko.Observable<boolean> | ko.Computed<boolean>, copyPasteStrategy?: ICopyPasteStrategy, zoomStep?: ko.Observable<number> | ko.Computed<number>);
            processShortcut(actions: Utils.IAction[], e: JQueryKeyEventObject): void;
            menuItems: Utils.IAction[];
        }
        interface ICopyPasteStrategy {
            createChild(pasteTarget: Elements.ElementViewModel, info: {}): Elements.ElementViewModel;
            calculateDelta(selection: ISelectionTarget, pasteTargetSurface: ISelectionTarget, minPoint: Elements.IPoint): {
                x: number;
                y: number;
            };
        }
        var copyPasteStrategy: ICopyPasteStrategy;
        class CopyPasteHandler {
            private _copyPasteStrategy;
            private _selectionProvider;
            private _copyInfo;
            constructor(selectionProvider: ISelectionProvider, _copyPasteStrategy?: ICopyPasteStrategy);
            hasPasteInfo: ko.PureComputed<boolean>;
            canCopy(): boolean;
            canPaste(): boolean;
            copy(): void;
            cut(): void;
            paste(): void;
        }
    }
    module Utils {
        interface ITabPanelOptions {
            tabs: TabInfo[];
            autoSelectTab?: boolean;
            rtl?: boolean;
        }
        class TabPanel extends Utils.Disposable {
            static Position: {
                Left: string;
                Right: string;
            };
            dispose(): void;
            constructor(options: ITabPanelOptions);
            private _resizableOptions;
            getResizableOptions: ($element: Element, panelOffset: string, minWidth: number) => any;
            tabs: TabInfo[];
            selectTab: (e: any) => void;
            isEmpty: ko.Observable<boolean> | ko.Computed<boolean>;
            collapsed: ko.Observable<boolean> | ko.Computed<boolean>;
            width: ko.Observable<number> | ko.Computed<number>;
            headerWidth: ko.Observable<number> | ko.Computed<number>;
            position: ko.Observable<string> | ko.Computed<string>;
            toggleCollapsedImage: ko.Computed<{
                class: string;
                template: string;
            }>;
            toggleCollapsedText: ko.PureComputed<any>;
            cssClasses: (extendOptions: {
                class: string;
                condition: any;
            }) => any;
        }
        interface ITabInfoOptions {
            text: string;
            template: string;
            model: any;
            localizationId?: string;
            imageClassName?: string;
            imageTemplateName?: string;
            visible?: ko.Observable<boolean> | ko.Computed<boolean>;
            disabled?: ko.Observable<boolean> | ko.Computed<boolean>;
        }
        class TabInfo extends Utils.Disposable {
            private _text;
            private _localizationId;
            constructor(options: ITabInfoOptions);
            imageClassName: ko.Observable<string> | ko.Computed<string>;
            imageTemplateName: string;
            active: ko.Observable<boolean> | ko.Computed<boolean>;
            visible: ko.Observable<boolean> | ko.Computed<boolean>;
            disabled: ko.Observable<boolean> | ko.Computed<boolean>;
            template: string;
            model: any;
            readonly text: any;
        }
        interface IToolboxItemInfo {
            "@ControlType": string;
            index: number;
            canDrop?: any;
            displayName?: string;
        }
        class ToolboxItem {
            constructor(info: Utils.IToolboxItemInfo);
            disabled: ko.Observable<boolean>;
            info: Utils.IToolboxItemInfo;
            readonly type: string;
            readonly imageClassName: string;
            readonly imageTemplateName: any;
            readonly index: number;
            readonly displayName: string;
        }
    }
    module Internal {
        function createObservableReverseArrayMapCollection<T>(elementModels: any, target: ko.ObservableArray<T>, createItem: (item: any) => T): any;
        function createObservableArrayMapCollection<T>(elementModels: any, target: ko.ObservableArray<T>, createItem: (item: any) => T): any;
        function deserializeChildArray<T>(model: any, parent: any, creator: any): ko.ObservableArray<T>;
        function getFirstItemByPropertyValue<T>(array: T[], propertyName: string, propertyValue: any, fromIndex?: number): T;
        function findFirstItemMatchesCondition<T>(array: T[], predicate: (item: T) => boolean): T;
        var find: typeof findFirstItemMatchesCondition;
        function binaryIndexOf<T>(ar: T[], el: T, compare: (a: T, b: T) => number): number;
        interface IDataSourceInfo {
            name: string;
            specifics?: string;
            id?: string;
            ref?: string;
            data: any;
            dataSerializer?: string;
            isSqlDataSource?: boolean;
            isJsonDataSource?: boolean;
        }
        interface IItemsExtender {
            beforeItemsFilled: (request: Utils.IPathRequest, items: Utils.IDataMemberInfo[]) => boolean;
            afterItemsFilled?: (request: Utils.IPathRequest, items: Utils.IDataMemberInfo[]) => void;
        }
        class FieldListProvider implements Utils.IItemsProvider {
            private _extenders;
            private _patchRequest;
            private _beforeFieldListCallback;
            private _afterFieldListCallBack;
            constructor(fieldListCallback: (pathRequest: Utils.IPathRequest) => JQueryPromise<Utils.IDataMemberInfo[]>, rootItems: ko.ObservableArray<IDataSourceInfo>, extenders?: IItemsExtender[]);
            getItems: (IPathRequest: any) => JQueryPromise<Utils.IDataMemberInfo[]>;
        }
        var NotifyType: {
            info: string;
            warning: string;
            error: string;
            success: string;
        };
        function NotifyAboutWarning(msg: any, showForUser?: boolean): void;
        function getErrorMessage(jqXHR: any): any;
        function ShowMessage(msg: string, type?: string, displayTime?: number, debugInfo?: string): void;
        function unitsToPixel(val: number, measureUnit: string, zoom?: number): number;
        function pixelToUnits(val: number, measureUnit: string, zoom: number): number;
        interface IUnitProperties<M> {
            [key: string]: (o: M) => ko.Observable<number> | ko.Computed<number>;
        }
        function createUnitProperty(model: any, target: any, propertyName: any, property: any, measureUnit: ko.Observable<string> | ko.Computed<string>, zoom: ko.Observable<number> | ko.Computed<number>, afterCreation?: (property: any) => void): void;
        function createUnitProperties<M>(model: M, target: any, properties: IUnitProperties<M>, measureUnit: ko.Observable<string> | ko.Computed<string>, zoom: ko.Observable<number> | ko.Computed<number>, afterCreation?: (property: any) => void): void;
        type SizeFactorType = "lg" | "md" | "sm" | "xs";
        function copyObservables(from: any, to: any): void;
        function compareObjects(a: any, b: any): boolean;
        var cssTransform: string[];
        var DEBUG: boolean;
        function getFullPath(path: string, dataMember: string): string;
        function loadTemplates(): any;
        function getSizeFactor(width: any): SizeFactorType;
        function appendStaticContextToRootViewModel(root: any, dx?: any): void;
        interface IAjaxSettings {
            uri: string;
            action: string;
            arg: any;
            processErrorCallback?: (message: string, jqXHR: any, textStatus: any) => void;
            ignoreError?: () => boolean;
            customOptions?: any;
            isError?: (data: any) => boolean;
            getErrorMessage?: (jqXHR: any) => string;
        }
        function _ajax(uri: any, action: any, arg: any, processErrorCallback?: (message: string, jqXHR: any, textStatus: any) => void, ignoreError?: () => boolean, customOptions?: any, isError?: (data: any) => boolean, getErrorMessage?: (jqXHR: any) => string): JQueryPromise<any>;
        function _ajaxWithOptions(options: IAjaxSettings): JQueryPromise<any>;
        function ajax(...params: (IAjaxSettings | any)[]): any;
        interface ICommonCustomizationHandler {
            customizeActions?: (actions: Utils.IAction[]) => void;
            customizeLocalization?: (callbacks?: JQueryPromise<any>[]) => void;
            onServerError?: (e: any) => void;
            beforeRender?: (designerModel: any) => void;
        }
        interface INamedValue {
            displayName: string;
            value: any;
        }
        function cutRefs(model: any): any;
        interface IDesignerPart {
            id: string;
            templateName: string;
            model: any;
        }
        var DesignerBaseElements: {
            MenuButton: string;
            Toolbar: string;
            Toolbox: string;
            Surface: string;
            RightPanel: string;
        };
        function generateDefaultParts(model: any): IDesignerPart[];
        function createActionWrappingFunction(wrapperName: string, func: (model: any, originalHandler: (model?: any) => any) => any): (actions: Utils.IAction[]) => void;
        function createDesigner(model: ko.Observable | ko.Computed, surface: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, controlsFactory: Utils.ControlsFactory, groups?: GroupObject, editors?: Utils.ISerializationInfoArray, parts?: IDesignerPart[], rtl?: boolean, selection?: Internal.SurfaceSelection, designControlsHelper?: Internal.DesignControlsHelper, undoEngine?: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, customMerge?: any, snapLinesCollector?: Internal.SnapLinesCollector, groupLocalizationIDs?: {
            [key: string]: string;
        }): IDesignerModel;
        function localizeNoneString(noneValue: any): any;
        function parseZoom(val: string): number;
        function objectsVisitor(target: any, visitor: (target: any) => any, visited?: any[], skip?: Array<string>): void;
        function collectionsVisitor(target: any, visitor: (target: any, owner?: any) => any, collectionsToProcess?: string[], visited?: any[]): void;
    }
    module Widgets {
        module Internal {
            class dxFieldListPicker extends dxDropDownBox {
                _path: ko.Observable<string>;
                _value: ko.Observable<string>;
                _parentViewport: JQuery;
                _itemsProvider: ko.Observable<any>;
                _defaultPosition: any;
                constructor($element: JQuery, options?: any);
                _showDropDown(): void;
                _getMaxHeight(): number;
                _closeOutsideDropDownHandler(e: any, ignoreContainerClicks: any): void;
                _hideOnBlur(): boolean;
                _popupConfig(): any;
                _setTitle(text: string): void;
                _optionChanged(obj: any, value: any): void;
                _clearValueHandler(): void;
                _renderPopupContent(): void;
            }
        }
    }
    module Internal {
        class BordersModel extends Utils.Disposable {
            private _setAllValues;
            setValue(name: any): void;
            setAll(): void;
            setNone(): void;
            updateModel(value: string): void;
            updateValue(): void;
            constructor(object: {
                value: ko.Observable<string>;
            }, disabled?: ko.Observable<boolean>);
            value: ko.Observable<string> | ko.Computed<string>;
            left: ko.Observable<boolean>;
            right: ko.Observable<boolean>;
            top: ko.Observable<boolean>;
            bottom: ko.Observable<boolean>;
            disabled: ko.Observable<boolean> | ko.Computed<boolean>;
        }
        class ControlProperties extends Widgets.ObjectProperties {
            getEditors(): Widgets.Editor[];
            protected _update(target: any, editorsInfo: any, recreateEditors: any): void;
            cleanEditors(): void;
            dispose(): void;
            createGroups(groups: GroupObject): void;
            constructor(target: ko.Observable<any>, editorsInfo?: {
                groups?: GroupObject;
                editors?: Utils.ISerializationInfoArray;
            }, level?: number, addAddons?: boolean);
            focusedItem: ko.Observable | ko.Computed;
            focusedImageClassName: ko.Observable<string> | ko.Computed<string>;
            displayExpr: (value: any) => string;
            popupService: Analytics.Internal.PopupService;
            groups: Group[];
            createEditorAddOn: (editor: Widgets.Editor) => Analytics.Internal.IEditorAddon;
            editorsRendered: ko.Observable<boolean> | ko.Computed<boolean>;
            isSortingByGroups: ko.Observable<boolean> | ko.Computed<boolean>;
            isSearching: ko.Observable<boolean> | ko.Computed<boolean>;
            allEditorsCreated: ko.Observable<boolean> | ko.Computed<boolean>;
            textToSearch: ko.Observable<string>;
            _searchBox: any;
            searchBox($element: JQuery): void;
            searchPlaceholder: () => any;
            switchSearchBox: () => void;
        }
        type GroupObject = {
            [key: string]: {
                info: Utils.ISerializationInfoArray;
                displayName?: () => string;
            };
        };
        class Group extends Utils.Disposable {
            private _viewModel;
            private _serializationsInfo;
            private _displayName;
            private _localizationId;
            constructor(name: string, serializationsInfo: Utils.ISerializationInfoArray, createEditors: (serializationInfo: Utils.ISerializationInfoArray) => Widgets.Editor[], collapsed?: boolean, displayName?: () => string);
            resetEditors(): void;
            dispose(): void;
            update(viewModel: Elements.ElementViewModel): void;
            displayName: () => string;
            editors: ko.ObservableArray<Widgets.Editor>;
            context: any;
            recreate: () => void;
            collapsed: ko.Observable<boolean> | ko.Computed<boolean>;
            visible: ko.Computed<Boolean>;
            editorsCreated: ko.Observable<boolean>;
            editorsRendered: ko.Observable<boolean>;
        }
        var sizeFake: Utils.ISerializationInfoArray;
        var locationFake: Utils.ISerializationInfoArray;
        interface IDesignerContext {
            model: ko.Observable | ko.Computed;
            surface?: ko.Observable | ko.Computed;
            undoEngine?: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>;
        }
        interface IDesignerModel extends Utils.IDisposable {
            model: ko.Observable | ko.Computed;
            rtl: boolean;
            surface: ko.Observable | ko.Computed;
            undoEngine: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>;
            selection: Internal.SurfaceSelection;
            snapHelper: Internal.SnapLinesHelper;
            editableObject: ko.Observable<any>;
            dragHelperContent: Internal.DragHelperContent;
            dragDropStarted: ko.Observable<boolean> | ko.Computed<boolean>;
            dragHandler: Internal.SelectionDragDropHandler;
            toolboxDragHandler: Internal.ToolboxDragDropHandler;
            resizeHandler: IResizeHandler;
            toolboxItems: Utils.ToolboxItem[];
            isLoading: ko.Observable<boolean> | ko.Computed<boolean>;
            propertyGrid: ControlProperties;
            popularProperties: Widgets.ObjectProperties;
            controlsHelper: Internal.DesignControlsHelper;
            controlsStore: Internal.ControlsStore;
            tabPanel: Utils.TabPanel;
            contextActionProviders: Internal.IActionsProvider[];
            contextActions: ko.Observable<Utils.IAction[]> | ko.Computed<Utils.IAction[]>;
            appMenuVisible: ko.Observable<boolean> | ko.Computed<boolean>;
            toggleAppMenu: () => void;
            getMenuPopupContainer: (el: HTMLElement) => JQuery;
            getMenuPopupTarget: (el: HTMLElement) => JQuery;
            inlineTextEdit: Internal.InlineTextEdit;
            actionsGroupTitle: () => string;
            updateFont: (values: {
                [key: string]: string;
            }) => void;
            sortFont: () => void;
            surfaceSize: ko.Observable<number> | ko.Computed<number>;
            popularVisible: ko.Computed<boolean>;
            actionLists: Internal.ActionLists;
            parts: IDesignerPart[];
            surfaceClass: (elem: any) => string;
        }
        class DesignerContextGeneratorInternal<T extends IDesignerContext> {
            private _context;
            private _rtl?;
            constructor(_context: T, _rtl?: boolean);
            addElement(propertyName: string, model: any): this;
            addUndoEngine(undoEngine?: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>): this;
            addSurface(surface: any): this;
            getContext(): T;
        }
        class DesignerContextGenerator<T extends IDesignerContext> {
            private _rtl?;
            constructor(_rtl?: boolean);
            addModel(model: any): DesignerContextGeneratorInternal<T>;
        }
        interface IDesingerGeneratorSettings {
            generate(): any;
        }
        interface IResizeHandler {
            starting: () => void;
            stopped: () => void;
            disabled?: ko.Observable<boolean> | ko.Computed<boolean>;
            snapHelper?: Internal.SnapLinesHelper;
        }
        class ResizeSettings extends Utils.Disposable implements IDesingerGeneratorSettings {
            private _handler;
            handler: IResizeHandler;
            generate(): {};
        }
        class ContextActionsSettings extends Utils.Disposable implements IDesingerGeneratorSettings {
            private _actionProviders;
            private _actions;
            private _actionUndoEngineWrappingFunction;
            actionProviders: IActionsProvider[];
            actions: ko.Observable<Utils.IAction[]> | ko.Computed<Utils.IAction[]>;
            createDefaultActions(editableObj: any, undoEngine: any): void;
            generate(): {};
        }
        class DragDropSettings extends Utils.Disposable implements IDesingerGeneratorSettings {
            private _model;
            private _dragHelperContent;
            private _dragDropStarted;
            dragHelperContent: DragHelperContent;
            dragDropStarted: boolean | ko.Observable<boolean>;
            addDragDropHandler(propertyName: string, handler: Internal.DragDropHandler): void;
            generate(): {};
        }
        class ControlsHelperSettings extends Utils.Disposable implements IDesingerGeneratorSettings {
            private _selection;
            private _context;
            private _model;
            private controlsHelper;
            constructor(_selection: Internal.SurfaceSelection, _context: IDesignerContext);
            generate(): {};
            addControlsHelper(helper?: Internal.IDesignControlsHelper): this;
            addControlsStore(store?: Internal.ControlsStore): this;
        }
        class MenuSettings extends Utils.Disposable implements IDesingerGeneratorSettings {
            generate(): {};
            private _appMenuVisible;
            toggleAppMenu: () => void;
            stopPropagation: boolean;
            getMenuPopupContainer: (el: HTMLElement) => JQuery;
            getMenuPopupTarget: (el: HTMLElement) => JQuery;
            appMenuVisible: ko.Computed<boolean> | ko.Observable<boolean>;
        }
        class SelectionSettings extends Utils.Disposable implements IDesingerGeneratorSettings {
            private _selection;
            private _snapHelper;
            private _editableObject;
            private _dragDropSettings;
            private _resizeSettings;
            dispose(): void;
            selection: SurfaceSelection;
            snapHelper: SnapLinesHelper;
            editableObject: any;
            addDragDrop(func: (settings: DragDropSettings) => void): void;
            addResize(func: (settings: ResizeSettings) => void): void;
            generate(): any;
        }
        class CommonDesignerGenerator<T extends IDesignerModel> extends Utils.Disposable {
            private _context?;
            private _rtl?;
            private _model;
            private _selectionSettings;
            private _createPopularProperties;
            private _resetModel;
            protected rtl: boolean;
            dispose(): void;
            constructor(_context?: IDesignerContext, _rtl?: boolean);
            initializeContext(context: IDesignerContext): this;
            getPropertyByName<T>(propertyName: string): T;
            addElement(propertyName: string, elementFunc: () => any): this;
            mapOnContext(): this;
            addSelection(func: (settings: SelectionSettings) => void): this;
            addPropertyGrid(propertyGrid?: () => Widgets.ObjectProperties, propertyName?: string): this;
            addControlProperties(editors: any, groups: GroupObject, groupLocalizationIDs: any): this;
            addPopularProperties(controlsFactory: any): this;
            addToolboxItems(items?: () => Utils.ToolboxItem[]): this;
            addTabPanel(panel?: () => Utils.TabPanel, addTabInfo?: () => Utils.TabInfo[]): this;
            addIsLoading(isLoadingFunc?: () => ko.Observable<boolean>): this;
            addControlsHelper(func: (settings: ControlsHelperSettings) => void): this;
            addMenu(func: (settings: MenuSettings) => void): this;
            addContextActions(func: (contextActions: ContextActionsSettings) => void): this;
            addParts(func?: (parts: any) => IDesignerPart[], useDefaults?: boolean): this;
            getModel(): T;
            addActionList(actionListsFunc?: () => Internal.ActionLists): this;
        }
        class dxButtonWithTemplate extends dxButton {
            constructor(element: any, options?: any);
            _getContentData(): any;
            _optionChanged(args: any): void;
        }
    }
}

export default DevExpress;