
import dxGallery from 'devextreme/ui/gallery';
import dxSelectBox from 'devextreme/ui/select_box';
/**
* DevExpress HTML/JS Analytics Core (dx-analytics-module-header.ts)
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

/**
* DevExpress HTML/JS Analytics Core (dx-analytics-core.d.ts)
* Version: 19.1.6
* Build date: 2019-09-10
* Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
* License: https://www.devexpress.com/Support/EULAs/NetComponents.xml
*/

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

/**
* DevExpress HTML/JS Query Builder (dx-querybuilder.d.ts)
* Version: 19.1.6
* Build date: 2019-09-10
* Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
* License: https://www.devexpress.com/Support/EULAs/NetComponents.xml
*/

declare module DevExpress.Analytics.Diagram {
    var name: Utils.ISerializationInfo;
    var text: Utils.ISerializationInfo;
    var size: Utils.ISerializationInfo;
    var location: Utils.ISerializationInfo;
    var sizeLocation: Utils.ISerializationInfoArray;
    var unknownSerializationsInfo: Utils.ISerializationInfoArray;
    class ConnectingPointDragHandler extends Internal.DragDropHandler {
        constructor(surface: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, selection: Internal.SurfaceSelection, undoEngine: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, snapHelper: Internal.SnapLinesHelper, dragHelperContent: Internal.DragHelperContent);
        startDrag(control: Internal.ISelectionTarget): void;
        drag(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
        doStopDrag(): void;
        startConnectingPoint: ConnectingPointSurface;
        newConnector: ConnectorViewModel;
        readonly newConnectorSurface: ConnectorSurface;
    }
    class ConnectionPointDragHandler extends Internal.DragDropHandler {
        constructor(surface: ko.Observable<Elements.ISurfaceContext> | ko.Computed<Elements.ISurfaceContext>, selection: Internal.SurfaceSelection, undoEngine: ko.Observable<Utils.UndoEngine> | ko.Computed<Utils.UndoEngine>, snapHelper: Internal.SnapLinesHelper, dragHelperContent: Internal.DragHelperContent);
        startDrag(control: Internal.ISelectionTarget): void;
        drag(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
        doStopDrag(): void;
        currentConnectionPoint: ConnectionPointSurface;
    }
    class DiagramElementBaseViewModel extends Elements.ElementViewModel {
        getControlFactory(): Utils.ControlsFactory;
        constructor(control: any, parent: Elements.ElementViewModel, serializer?: Utils.ModelSerializer);
        size: Elements.Size;
        location: Elements.Point;
    }
    class DiagramElementViewModel extends DiagramElementBaseViewModel {
        constructor(control: any, parent: Elements.ElementViewModel, serializer?: Utils.ModelSerializer);
        connectingPoints: ko.ObservableArray<ConnectingPointViewModel>;
        text: ko.Observable<string> | ko.Computed<string>;
        type: ko.Observable<string> | ko.Computed<string>;
    }
    var diagramElementSerializationInfo: Utils.ISerializationInfoArray;
    class DiagramElementBaseSurface<M extends DiagramElementBaseViewModel> extends Elements.SurfaceElementBase<M> {
        static _unitProperties: Internal.IUnitProperties<DiagramElementBaseViewModel>;
        constructor(control: M, context: Elements.ISurfaceContext, unitProperties: Internal.IUnitProperties<M>);
        template: string;
        selectiontemplate: string;
        contenttemplate: string;
        margin: ko.Observable<number>;
        positionWidthWithoutMargins: ko.Computed<number>;
        positionLineHeightWithoutMargins: ko.Computed<number>;
    }
    class DiagramElementSurface extends DiagramElementBaseSurface<DiagramElementViewModel> {
        constructor(control: DiagramElementViewModel, context: Elements.ISurfaceContext);
        _getChildrenHolderName(): string;
        contenttemplate: string;
    }
    class DiagramViewModel extends DiagramElementBaseViewModel {
        getInfo(): Utils.ISerializationInfoArray;
        constructor(diagramSource: any);
        controls: ko.ObservableArray<DiagramElementViewModel>;
        name: ko.Observable<string> | ko.Computed<string>;
        pageWidth: ko.Observable<number> | ko.Computed<number>;
        pageHeight: ko.Observable<number> | ko.Computed<number>;
        margins: Elements.Margins;
    }
    var margins: Utils.ISerializationInfo;
    var pageWidth: Utils.ISerializationInfo;
    var pageHeight: Utils.ISerializationInfo;
    var diagramSerializationsInfo: Utils.ISerializationInfoArray;
    class DiagramSurface extends Elements.SurfaceElementBase<DiagramViewModel> implements Internal.ISelectionTarget, Elements.ISurfaceContext {
        static _unitProperties: Internal.IUnitProperties<DiagramViewModel>;
        constructor(diagram: DiagramViewModel, zoom?: ko.Observable<number>);
        measureUnit: ko.Observable<string>;
        dpi: ko.Observable<number>;
        zoom: ko.Observable<number> | ko.Computed<number>;
        controls: ko.ObservableArray<DiagramElementSurface>;
        allowMultiselect: boolean;
        focused: ko.Observable<boolean>;
        selected: ko.Observable<boolean>;
        underCursor: ko.Observable<Internal.IHoverInfo>;
        checkParent(surfaceParent: Internal.ISelectionTarget): boolean;
        parent: Internal.ISelectionTarget;
        templateName: string;
        getChildrenCollection(): ko.ObservableArray<any>;
        margins: Elements.IMargins;
    }
    class ConnectionPointViewModel extends DiagramElementBaseViewModel {
        constructor(control: any, parent: ConnectorViewModel, serializer?: Utils.ModelSerializer);
        location: Elements.Point;
        connectingPoint: ko.Observable<IConnectingPoint>;
    }
    var connectionPointSerializationInfo: Utils.ISerializationInfoArray;
    class ConnectionPointSurface extends Elements.SurfaceElementBase<ConnectionPointViewModel> {
        static _unitProperties: Internal.IUnitProperties<ConnectionPointViewModel>;
        constructor(control: ConnectionPointViewModel, context: Elements.ISurfaceContext);
        template: string;
        selectiontemplate: string;
        relativeX: ko.Observable<number> | ko.Computed<number>;
        relativeY: ko.Observable<number> | ko.Computed<number>;
        container(): DiagramSurface;
    }
    class ConnectorViewModel extends DiagramElementBaseViewModel {
        static MIN_LINE_THICKNESS: number;
        getX(): number;
        getY(): number;
        getWidth(): number;
        getHeight(): number;
        constructor(control: any, parent: Elements.ElementViewModel, serializer?: Utils.ModelSerializer);
        startPoint: ko.Observable<ConnectionPointViewModel> | ko.Computed<ConnectionPointViewModel>;
        endPoint: ko.Observable<ConnectionPointViewModel> | ko.Computed<ConnectionPointViewModel>;
    }
    class ConnectorSurface extends DiagramElementBaseSurface<ConnectorViewModel> {
        constructor(control: ConnectorViewModel, context: Elements.ISurfaceContext);
        template: string;
        selectiontemplate: string;
        startPoint: ko.Observable<ConnectionPointSurface> | ko.Computed<ConnectionPointSurface>;
        endPoint: ko.Observable<ConnectionPointSurface> | ko.Computed<ConnectionPointSurface>;
    }
    class RoutedConnectorViewModel extends ConnectorViewModel {
        static GRID_SIZE: number;
        private _isUpdating;
        getX(): number;
        getY(): number;
        getWidth(): number;
        getHeight(): number;
        _fixPoint(point: Elements.IPoint, side: PointSide): void;
        _getStartPointSide(): PointSide;
        _getEndPointSide(): PointSide;
        private _getPower;
        private _getRatio;
        constructor(control: any, parent: Elements.ElementViewModel, serializer?: Utils.ModelSerializer);
        seriesNumber: ko.Observable<number>;
        routePoints: ko.Observable<Elements.IPoint[]>;
        freezeRoute: ko.Observable<boolean>;
        beginUpdate(): void;
        endUpdate(): void;
    }
    interface IRoutePoint {
        x: ko.Observable<number> | ko.Computed<number>;
        y: ko.Observable<number> | ko.Computed<number>;
        modelPoint: Elements.IPoint;
    }
    class RoutedConnectorSurface extends DiagramElementBaseSurface<RoutedConnectorViewModel> {
        private static _connectorsCount;
        private _connectorID;
        private _createRoutePoint;
        private _createRouteLineWrapper;
        private _updateRoutePoints;
        constructor(control: RoutedConnectorViewModel, context: Elements.ISurfaceContext);
        template: string;
        selectiontemplate: string;
        startPoint: ko.Observable<ConnectionPointSurface> | ko.Computed<ConnectionPointSurface>;
        endPoint: ko.Observable<ConnectionPointSurface> | ko.Computed<ConnectionPointSurface>;
        showArrow: ko.Observable<boolean> | ko.Computed<boolean>;
        isVisible: ko.Observable<boolean> | ko.Computed<boolean>;
        routePoints: ko.ObservableArray<IRoutePoint>;
        routePointsSet: ko.PureComputed<string>;
        routeLineWrappers: ko.PureComputed<any[]>;
        connectorID: () => number;
    }
    interface IConnectingPoint {
        location: Elements.IPoint;
        side: ko.Observable<PointSide> | ko.Computed<PointSide>;
    }
    class ConnectingPointViewModel extends DiagramElementBaseViewModel implements IConnectingPoint {
        constructor(control: any, parent: DiagramElementViewModel, serializer?: Utils.ModelSerializer);
        percentOffsetX: ko.Observable<number> | ko.Computed<number>;
        percentOffsetY: ko.Observable<number> | ko.Computed<number>;
        side: ko.PureComputed<PointSide>;
    }
    var connectingPointSerializationInfo: Utils.ISerializationInfoArray;
    class ConnectingPointSurface extends DiagramElementBaseSurface<ConnectingPointViewModel> {
        static _unitProperties: Internal.IUnitProperties<ConnectingPointViewModel>;
        constructor(control: ConnectingPointViewModel, context: Elements.ISurfaceContext);
        template: string;
        selectiontemplate: string;
        contenttemplate: string;
    }
    var controlsFactory: Utils.ControlsFactory;
    function registerControls(): void;
    var groups: Internal.GroupObject;
    function createDiagramDesigner(element: Element, diagramSource: ko.Observable<any>, localization?: any, rtl?: boolean): any;
    enum PointSide {
        East = 0,
        South = 1,
        North = 2,
        West = 3
    }
    function determineConnectingPoints<T extends {
        rightConnectionPoint: Diagram.IConnectingPoint;
        leftConnectionPoint: Diagram.IConnectingPoint;
    }>(startObject: T, endObject: T): {
        start: Diagram.IConnectingPoint;
        end: Diagram.IConnectingPoint;
    };
}

declare module DevExpress {
    module Analytics {
        module Data {
            interface IDBSchemaProvider extends Analytics.Utils.IItemsProvider {
                getDbSchema: () => JQueryPromise<DBSchema>;
                getDbTable: (tableName: string) => JQueryPromise<DBTable>;
                getDbStoredProcedures: () => JQueryPromise<DBStoredProcedure[]>;
            }
            module Internal {
                function getDBSchemaCallback(requestWrapper: DevExpress.QueryBuilder.Utils.RequestWrapper, connection: SqlDataConnection, tables: DBTable[]): JQueryPromise<DBSchema>;
                function getDBStoredProceduresCallback(requestWrapper: DevExpress.QueryBuilder.Utils.RequestWrapper, connection: SqlDataConnection): JQueryPromise<DBStoredProcedure[]>;
            }
            class DBSchemaProvider extends Analytics.Utils.Disposable implements IDBSchemaProvider {
                private _requestWrapper;
                private _dbSchema;
                private _dbStoredProceduresSchema;
                private _tables;
                private _tableRequests;
                connection: SqlDataConnection;
                private _getDBSchema;
                private _getDBSchemaCallback;
                private _getDBStoredProcedures;
                constructor(connection: SqlDataConnection, _requestWrapper?: QueryBuilder.Utils.RequestWrapper);
                getItems: (IPathRequest: any) => JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
                getDbSchema(): JQueryPromise<DBSchema>;
                getDbStoredProcedures(): JQueryPromise<DBStoredProcedure[]>;
                getDbTable(tableName: string): JQueryPromise<DBTable>;
            }
            class DBStoredProcedure {
                name: string;
                arguments: DBStoredProcedureArgument[];
                constructor(model: any);
            }
            enum DBStoredProcedureArgumentDirection {
                In = 0,
                Out = 1,
                InOut = 2
            }
            class DBStoredProcedureArgument {
                name: string;
                type: DBColumnType;
                direction: DBStoredProcedureArgumentDirection;
                constructor(model: any);
            }
            class DBTable {
                name: string;
                columns: DBColumn[];
                isView: boolean;
                foreignKeys: DBForeignKey[];
                constructor(model: any);
            }
            class ResultSet {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): ResultSet;
                static toJson(value: any, serializer: any, refs: any): {
                    "DataSet": any;
                };
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                tables: ko.ObservableArray<ResultTable>;
            }
            class ResultTable {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                tableName: ko.Observable<string> | ko.Computed<string>;
                columns: ko.ObservableArray<{
                    name: ko.Observable<string> | ko.Computed<string>;
                    propertyType: ko.Observable<string>;
                }>;
            }
            module Utils {
                var SqlQueryType: {
                    customSqlQuery: string;
                    tableQuery: string;
                    storedProcQuery: string;
                };
                var JsonSourceType: {
                    fileJsonSource: string;
                    customJsonSource: string;
                    uriJsonSource: string;
                };
                interface ISqlQueryViewModel extends Analytics.Utils.ISerializableModel {
                    name: ko.Observable<string> | ko.Computed<string>;
                    parameters: ko.ObservableArray<DataSourceParameter>;
                    type: ko.Observable<string> | ko.Computed<string>;
                    parent: SqlDataSource;
                    generateName: () => string;
                }
            }
            module Internal {
                function generateQueryUniqueName(queries: Utils.ISqlQueryViewModel[], query: Utils.ISqlQueryViewModel): string;
            }
            enum JsonNodeType {
                Object = 0,
                Array = 1,
                Property = 2
            }
            class JsonNode {
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonNode;
                static toJsonNodes(value: JsonNode[], serializer: any, refs: any): any[];
                static toJsonNode(value: JsonNode, serializer: any, refs: any, recoursive?: boolean): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                name: ko.Observable<string> | ko.Computed<string>;
                nodes: JsonNode[];
                selected: ko.Observable<boolean> | ko.Computed<boolean>;
                value: any;
                nodeType: string;
                valueType: string;
                displayName: string;
            }
            class JsonSchemaNode extends JsonNode {
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonSchemaNode;
                static toJson(value: JsonSchemaNode, serializer: any, refs: any): {};
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                nodeType: string;
                valueType: string;
                displayName: any;
                selected: ko.Observable<boolean>;
            }
            class JsonSchemaRootNode extends JsonNode {
                private _rootElementList;
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonSchemaRootNode;
                static toJson(value: JsonSchemaRootNode, serializer: any, refs: any): {};
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                getRootElementPartList(): Analytics.Utils.IPathRequest[];
                private _fillRootElementList;
                private _getNextPath;
            }
            class JsonAuthenticationInfo {
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonAuthenticationInfo;
                static toJson(value: JsonAuthenticationInfo, serializer: any, refs: any): any;
                getInfo(): {
                    propertyName: string;
                    modelName: string;
                    defaultVal: string;
                }[];
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                password: ko.Observable<string> | ko.Computed<string>;
                userName: ko.Observable<string> | ko.Computed<string>;
            }
            class JsonParameter extends Analytics.Utils.Disposable {
                static toJson(value: JsonParameter, serializer: any, refs: any): any;
                getInfo(): ({
                    propertyName: string;
                    modelName: string;
                    displayValue: string;
                    editor: any;
                } | {
                    propertyName: string;
                    modelName: string;
                    displayValue?: undefined;
                    editor?: undefined;
                })[];
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                itemType: ko.Observable<string> | ko.Computed<string>;
                name: ko.Observable<string> | ko.Computed<string>;
                namePlaceholder: () => any;
                valuePlaceholder: () => any;
                value: ko.Observable<string> | ko.Computed<string>;
            }
            class JsonHeaderParameter extends JsonParameter {
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonHeaderParameter;
                itemType: ko.Observable<string>;
            }
            class JsonQueryParameter extends JsonParameter {
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonQueryParameter;
                itemType: ko.Observable<string>;
            }
            class JsonSource extends Analytics.Utils.Disposable {
                private static _URIJSONSOURCE_TYPE;
                private static _CUSTOMJSONSOURCE_TYPE;
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonSource;
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model?: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                sourceType: ko.Observable<string>;
                uri: ko.Observable<string>;
                json: ko.Observable<string>;
                authenticationInfo: JsonAuthenticationInfo;
                headers: ko.ObservableArray<JsonHeaderParameter>;
                queryParameters: ko.ObservableArray<JsonQueryParameter>;
                serialize(includeRootTag?: boolean): any;
                resetSource(): void;
            }
            class JsonDataSource extends Analytics.Utils.Disposable implements IDataSourceBase {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                clone(serializer?: Analytics.Utils.IModelSerializer): JsonDataSource;
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): JsonDataSource;
                static toJson(value: any, serializer: any, refs: any): any;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer, requestWrapper?: QueryBuilder.Utils.RequestWrapper);
                getSchema(): JQueryPromise<JsonSchemaRootNode>;
                name: ko.Observable<string> | ko.Computed<string>;
                id: string;
                connectionName: ko.Observable<string> | ko.Computed<string>;
                jsonSchemaProvider: JsonSchemaProvider;
                schema: JsonSchemaRootNode;
                rootElement: ko.Observable<string> | ko.Computed<string>;
                source: JsonSource;
            }
            interface IJsonSchemaProvider extends Analytics.Utils.IItemsProvider {
                getJsonSchema: () => JQueryPromise<JsonSchemaRootNode>;
            }
            module Internal {
                function getJsonSchemaCallback(requestWrapper: QueryBuilder.Utils.RequestWrapper, jsonDataSource: JsonDataSource): JQueryPromise<JsonSchemaRootNode>;
            }
            class JsonSchemaProvider extends Analytics.Utils.Disposable implements IJsonSchemaProvider {
                private _requestWrapper;
                private _jsonSchemaPromise;
                private _jsonDataSource;
                private _jsonSchema;
                private _getJsonSchema;
                private _getJsonSchemaCallback;
                constructor(jsonDataSource: JsonDataSource, _requestWrapper?: QueryBuilder.Utils.RequestWrapper);
                reset(): void;
                mapToDataMemberContract(nodes: JsonNode[]): Analytics.Utils.IDataMemberInfo[];
                getSchemaByPath(pathRequest: Analytics.Utils.IPathRequest, jsonSchema: JsonSchemaNode): Analytics.Utils.IDataMemberInfo[];
                getItems: (IPathRequest: any) => JQueryPromise<Analytics.Utils.IDataMemberInfo[]>;
                getJsonSchema(): JQueryPromise<JsonSchemaRootNode>;
            }
            class ConnectionOptions {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                closeConnection: ko.Observable<boolean>;
                commandTimeout: ko.Observable<number>;
            }
            module Metadata {
                var customQuerySerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class CustomSqlQuery implements Utils.ISqlQueryViewModel {
                parent: SqlDataSource;
                constructor(model: any, parent: SqlDataSource, serializer?: Analytics.Utils.IModelSerializer);
                sqlString: ko.Observable<string> | ko.Computed<string>;
                name: ko.Observable<string> | ko.Computed<string>;
                type: ko.Observable<string> | ko.Computed<string>;
                parameters: ko.ObservableArray<DataSourceParameter>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                generateName(): string;
            }
            module Metadata {
                var masterDetailRelationSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class MasterDetailRelation extends Analytics.Utils.Disposable {
                dispose(): void;
                private _customName;
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                name: ko.PureComputed<string>;
                masterQuery: ko.Observable<string> | ko.Computed<string>;
                detailQuery: ko.Observable<string> | ko.Computed<string>;
                keyColumns: ko.ObservableArray<{
                    masterColumn: ko.Observable<string> | ko.Computed<string>;
                    detailColumn: ko.Observable<string> | ko.Computed<string>;
                }>;
                createKeyColumn(): void;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class SqlDataConnection {
                static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): SqlDataConnection;
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                name: ko.Observable<string>;
                parameteres: ko.Observable<string>;
                fromAppConfig: ko.Observable<boolean>;
                options: ConnectionOptions;
            }
            interface IDataSourceBase {
                name: ko.Observable<string> | ko.Computed<string>;
                id: string;
            }
            class SqlDataSource extends Analytics.Utils.Disposable implements IDataSourceBase {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                createQuery(item: any, serializer: any): Utils.ISqlQueryViewModel;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer, requestWrapper?: QueryBuilder.Utils.RequestWrapper);
                name: ko.Observable<string> | ko.Computed<string>;
                id: string;
                queries: ko.ObservableArray<Utils.ISqlQueryViewModel>;
                relations: ko.ObservableArray<MasterDetailRelation>;
                connection: SqlDataConnection;
                dbSchemaProvider: DBSchemaProvider;
                resultSet: ResultSet;
            }
            module Metadata {
                var storedProcQuerySerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class StoredProcQuery implements Utils.ISqlQueryViewModel {
                parent: SqlDataSource;
                constructor(model: any, parent: SqlDataSource, serializer?: Analytics.Utils.IModelSerializer);
                procName: ko.Observable<string> | ko.Computed<string>;
                name: ko.Observable<string> | ko.Computed<string>;
                type: ko.Observable<string> | ko.Computed<string>;
                parameters: ko.ObservableArray<DataSourceParameter>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                generateName(): string;
            }
            module Metadata {
                var tableQuerySerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class TableQuery implements Utils.ISqlQueryViewModel {
                parent: SqlDataSource;
                constructor(model: any, parent: SqlDataSource, serializer?: Analytics.Utils.IModelSerializer);
                name: ko.Observable<string> | ko.Computed<string>;
                type: ko.Observable<string> | ko.Computed<string>;
                filterString: ko.Observable<string> | ko.Computed<string>;
                parameters: ko.ObservableArray<DataSourceParameter>;
                tables(): {
                    name: ko.Observable<string> | ko.Computed<string>;
                    alias: ko.Observable<string>;
                }[];
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                generateName(): string;
            }
            module Metadata {
                var dsParameterNameValidationRules: Array<any>;
                var parameterValueSerializationsInfo: {
                    propertyName: string;
                    displayName: string;
                    localizationId: string;
                    editor: any;
                };
                var dsParameterSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                function storedProcParameterSerializationsInfo(type: string): any[];
            }
            class DataSourceParameter extends Analytics.Utils.Disposable implements Analytics.Utils.ISerializableModel {
                private _serializationsInfo;
                private _name;
                static typeValues: any[];
                private _getTypeValue;
                private _tryConvertValue;
                private static _isValueValid;
                getEditorType(type: any): {
                    header?: any;
                    content?: any;
                    custom?: any;
                };
                private _updateValueInfo;
                private _valueInfo;
                private _value;
                private _expressionValue;
                private _previousResultType;
                private _parametersFunctions;
                constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer, _serializationsInfo?: Analytics.Utils.ISerializationInfoArray);
                readonly specifics: any;
                static validateName(nameCandidate: string): boolean;
                isValid: ko.Observable<boolean> | ko.Computed<boolean>;
                name: ko.Computed<string>;
                value: ko.Observable | ko.Computed;
                type: ko.Observable<string> | ko.Computed<string>;
                resultType: ko.Observable<string> | ko.Computed<string>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyVisible(propName: string): boolean;
            }
            enum DBColumnType {
                Unknown = 0,
                Boolean = 1,
                Byte = 2,
                SByte = 3,
                Char = 4,
                Decimal = 5,
                Double = 6,
                Single = 7,
                Int32 = 8,
                UInt32 = 9,
                Int16 = 10,
                UInt16 = 11,
                Int64 = 12,
                UInt64 = 13,
                String = 14,
                DateTime = 15,
                Guid = 16,
                TimeSpan = 17,
                ByteArray = 18
            }
            class DBColumn {
                name: string;
                type: DBColumnType;
                size: string;
                constructor(model: any);
                static GetType(dbColumnType: DBColumnType): "System.String" | "System.DateTime" | "System.Int16" | "System.Int32" | "System.Int64" | "System.Single" | "System.Double" | "System.Decimal" | "System.Boolean" | "System.Guid" | "System.SByte" | "System.Byte" | "System.UInt16" | "System.UInt32" | "System.UInt64" | "System.Char" | "System.TimeSpan" | "System.Byte[]" | "System.Object";
                static GetSpecific(type: string): "String" | "Date" | "Bool" | "Integer" | "Float";
            }
            class DBForeignKey {
                name: string;
                primaryKeyTable: string;
                column: string;
                primaryKeyColumn: string;
                constructor(model: any);
            }
            module Internal {
                function deserializeToCollection<T>(model: any[], createItem: (itemModel: any) => T, collection?: T[]): T[];
            }
            class DBSchema {
                tables: DBTable[];
                procedures: DBStoredProcedure[];
                constructor(model: any);
            }
        }
        module Wizard {
            var DataSourceWizardPageId: {
                ChooseDataSourceTypePage: string;
                ConfigureMasterDetailRelationshipsPage: string;
            };
            var SqlDataSourceWizardPageId: {
                ChooseConnectionPage: string;
                ConfigureQueryPage: string;
                ConfigureParametersPage: string;
                MultiQueryConfigurePage: string;
                MultiQueryConfigureParametersPage: string;
            };
            var JsonDataSourceWizardPageId: {
                ChooseJsonSourcePage: string;
                ChooseJsonSchemaPage: string;
                ChooseConnectionPage: string;
                SpecifyJsonConnectionPage: string;
            };
            var FullscreenDataSourceWizardPageId: {
                ChooseDataSourceTypePage: string;
                SpecifySqlDataSourceSettingsPage: string;
                SpecifyJsonDataSourceSettingsPage: string;
            };
            var FullscreenDataSourceWizardSectionId: {
                SpecifyJsonConnectionPage: string;
                ChooseJsonSchemaPage: string;
                ChooseJsonSourcePage: string;
                ChooseSqlConnectionPage: string;
                ConfigureQueryPage: string;
                ConfigureQueryParametersPage: string;
                ConfigureMasterDetailRelationshipsPage: string;
            };
            interface ISqlDataSourceWizardState {
                name?: string;
                queryName?: string;
                sqlDataSourceJSON?: string;
                relations?: string[];
                customQueries?: string[];
            }
            interface IJsonDataSourceWizardState {
                dataSourceName?: string;
                jsonScheme?: string;
                rootElement?: string;
                jsonSource?: string;
                newConnectionName?: string;
                connectionName?: string;
            }
            interface IDataSourceWizardState {
                dataSourceType?: DataSourceType;
                sqlDataSourceWizard?: ISqlDataSourceWizardState;
                jsonDataSourceWizard?: IJsonDataSourceWizardState;
                dataSourceId?: string;
            }
            function _restoreSqlDataSourceFromState(state?: ISqlDataSourceWizardState, requestWrapper?: QueryBuilder.Utils.RequestWrapper, dataSourceId?: string): _SqlDataSourceWrapper;
            function _restoreJsonDataSourceFromState(state: IJsonDataSourceWizardState, requestWrapper?: QueryBuilder.Utils.RequestWrapper, dataSourceId?: string): Data.JsonDataSource;
            function _createDefaultDataSourceWizardState(sqlDataSourceWizardState?: ISqlDataSourceWizardState, jsonDataSourceWizardState?: IJsonDataSourceWizardState): IDataSourceWizardState;
            interface IWizardPageMetadata<T extends IWizardPage> {
                setState: (data: any, state: any) => void;
                getState: (state: any) => any;
                resetState: (state: any, defaultState?: any) => void;
                create: () => T;
                canNext?: (page: T) => boolean;
                canFinish?: (page: T) => boolean;
                template: string;
                description?: string;
            }
            interface IWizardPage extends Utils.IDisposable {
                commit: () => JQueryPromise<any>;
                initialize: (state: any) => JQueryPromise<any>;
                canFinish: () => boolean;
                canNext: () => boolean;
                onChange?: (callback: () => void) => void;
            }
            class WizardPageBase<TState = any, TResult = any> extends Utils.Disposable implements IWizardPage {
                dispose(): void;
                commit(): JQueryPromise<any>;
                protected _onChange: () => void;
                onChange(callback: any): void;
                initialize(state: TState): JQueryPromise<any>;
                canNext(): boolean;
                canFinish(): boolean;
            }
            class _WrappedWizardPage extends Utils.Disposable {
                pageId: string;
                page: IWizardPage;
                template: string;
                description?: string;
                dispose(): void;
                resetCommitedState(): void;
                private _lastCommitedState;
                private _isInitialized;
                private _initDef;
                isChanged: boolean;
                onChange: (callback: () => void) => void;
                constructor(pageId: string, page: IWizardPage, template: string, description?: string);
                commit(): JQueryPromise<any>;
                initialize(state: any, force?: boolean): JQueryPromise<any>;
            }
            interface ITypeItem {
                text: string;
                imageClassName: string;
                imageTemplateName: string;
                type: number;
            }
            enum DataSourceType {
                NoData = 0,
                Sql = 1,
                Json = 2
            }
            class TypeItem implements ITypeItem {
                constructor(textDefault: string, textID: string, imageClassName: string, imageTemplateName: string, type: number);
                text: string;
                imageClassName: string;
                imageTemplateName: string;
                type: number;
            }
            class ChooseDataSourceTypePage extends WizardPageBase<IDataSourceWizardState, IDataSourceWizardState> {
                constructor(dataSourceTypeOptions: _DataSourceWizardOptions);
                canNext(): boolean;
                canFinish(): boolean;
                _itemClick: (item: ITypeItem) => void;
                _IsSelected: (item: ITypeItem) => boolean;
                _goToNextPage(): void;
                commit(): JQueryPromise<IDataSourceWizardState>;
                initialize(state: IDataSourceWizardState): JQueryPromise<{}>;
                typeItems: ITypeItem[];
                selectedItem: ko.Observable<ITypeItem>;
                _extendCssClass: (rightPath: string) => string;
            }
            function _registerChooseDataSourceTypePage(factory: PageFactory, dataSourceTypeOptions: _DataSourceWizardOptions): void;
            interface IConnectionStringDefinition {
                name: string;
                description?: string;
            }
            class ChooseSqlConnectionPage extends WizardPageBase<ISqlDataSourceWizardState, ISqlDataSourceWizardState> {
                constructor(connectionStrings: ko.ObservableArray<IConnectionStringDefinition>);
                initialize(state: ISqlDataSourceWizardState): JQueryPromise<{}>;
                canNext(): boolean;
                commit(): JQueryPromise<ISqlDataSourceWizardState>;
                _connectionStrings: ko.ObservableArray<IConnectionStringDefinition>;
                _selectedConnectionString: ko.ObservableArray<IConnectionStringDefinition>;
            }
            function _registerChooseSqlConnectionPage(factory: PageFactory, connectionStrings: ko.ObservableArray<IConnectionStringDefinition>): void;
            interface IChooseAvailableItemPageOperation {
                text: string;
                createNew: boolean;
            }
            class ChooseAvailableItemPage extends WizardPageBase {
                items: ko.Subscribable<any[]>;
                constructor(items: ko.Subscribable<any[]>, canCreateNew?: boolean);
                canNext(): boolean;
                canCreateNew: ko.Observable<boolean>;
                selectedItems: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>;
                operations: IChooseAvailableItemPageOperation[];
                selectedOperation: ko.Observable<IChooseAvailableItemPageOperation>;
                _createNew: ko.PureComputed<boolean>;
                initialize(state: any): any;
                _displayExpr(item: any): any;
                _getSelectedItem(state?: any): Analytics.Internal.IDataSourceInfo;
                readonly createNewOperationText: any;
                readonly existingOperationText: any;
            }
            class ChooseJsonConnectionPage extends ChooseAvailableItemPage {
                commit(): JQueryPromise<IJsonDataSourceWizardState>;
                _getSelectedItem(data: IJsonDataSourceWizardState): Analytics.Internal.IDataSourceInfo;
                readonly createNewOperationText: any;
                readonly existingOperationText: any;
            }
            function _registerChooseJsonConnectionPage(factory: PageFactory, wizardOptions: _DataSourceWizardOptions): void;
            class SpecifyJsonConnectionPage extends ChooseJsonConnectionPage {
                constructor(connections: any, canCreateNewJsonDataSource: any);
                commit(): JQueryPromise<IJsonDataSourceWizardState>;
                canNext(): boolean;
                initialize(state: any): JQueryPromise<IWizardPage>;
                _specifySourceData: ChooseJsonSourcePage;
            }
            function _registerSpecifyJsonConnectionPage(factory: PageFactory, connections: ko.ObservableArray<any>, canCreateNewJsonDataSource: boolean): void;
            class ChooseJsonSourcePage extends WizardPageBase<IJsonDataSourceWizardState, IJsonDataSourceWizardState> {
                private _requestWrapper;
                private _jsonStringSettings;
                private _jsonUriSetting;
                private __validationGroup;
                private __validationSummary;
                private _onValidationGroupInitialized;
                private _onValidationGroupDisposing;
                private _onValidationSummaryInitialized;
                private _onValidationSummaryDisposing;
                constructor();
                canNext(): boolean;
                commit(): JQueryPromise<IJsonDataSourceWizardState>;
                initialize(state: IJsonDataSourceWizardState): JQueryPromise<{}>;
                _jsonSourceTitle: any;
                _jsonConnectionTitle: any;
                _connectionNameValidationRules: {
                    type: string;
                    message: any;
                }[];
                _connectionName: ko.Observable<string>;
                _validationGroup: {
                    onInitialized: (args: any) => void;
                    onDisposing: (args: any) => void;
                };
                _validationSummary: {
                    onInitialized: (args: any) => void;
                    onDisposing: (args: any) => void;
                };
                _sources: Array<Analytics.Wizard.IJsonDataSourceType>;
                _selectedSource: ko.PureComputed;
            }
            function _registerChooseJsonSourcePage(factory: PageFactory): void;
            class ChooseJsonSchemaPage extends WizardPageBase<IJsonDataSourceWizardState, IJsonDataSourceWizardState> {
                private _requestWrapper;
                private _rootItems;
                private _fieldListItemsProvider;
                private _fieldSelectedPath;
                private _dataSource;
                private _cachedState;
                private _clear;
                private _createFieldListCallback;
                private _getSchemaToDataMemberInfo;
                private _mapJsonNodesToTreelistItems;
                private _getNodesByPath;
                private _getInnerItemsByPath;
                private _beginInternal;
                private _updatePage;
                private _createTreeNode;
                private _createLeafTreeNode;
                private _resetSelectionRecursive;
                private _mapJsonSchema;
                canNext(): boolean;
                canFinish(): boolean;
                constructor(_requestWrapper?: QueryBuilder.Utils.RequestWrapper);
                commit(): JQueryPromise<IJsonDataSourceWizardState>;
                initialize(state: IJsonDataSourceWizardState): JQueryPromise<{}>;
                reset(): void;
                _rootElementTitle: any;
                _rootElementList: ko.Observable<Utils.IPathRequest[]>;
                _selectedRootElement: ko.Observable<Utils.IPathRequest>;
                _fieldListModel: Analytics.Widgets.Internal.ITreeListOptions;
            }
            function _registerChooseJsonSchemaPage(factory: PageFactory, requestWrapper?: QueryBuilder.Utils.RequestWrapper): void;
            class _SqlDataSourceWrapper {
                sqlDataSourceJSON?: string;
                sqlDataSource: Data.SqlDataSource;
                private _queryIndex;
                sqlQuery: Data.Utils.ISqlQueryViewModel;
                saveCustomQueries(): string[];
                save(): string;
                customQueries: Data.Utils.ISqlQueryViewModel[];
                constructor(sqlDataSourceJSON?: string, queryName?: string, requestWrapper?: QueryBuilder.Utils.RequestWrapper);
            }
            class ConfigureQueryPage extends WizardPageBase<ISqlDataSourceWizardState, ISqlDataSourceWizardState> {
                private _options;
                static QUERY_TEXT: string;
                static SP_TEXT: string;
                private _proceduresList;
                private _selectStatementControl;
                private _dataSourceWrapper;
                private _connection;
                private _dataSource;
                constructor(_options: _DataSourceWizardOptions);
                canNext(): boolean;
                canFinish(): boolean;
                runQueryBuilder(): void;
                localizeQueryType(queryTypeString: string): any;
                initialize(state: ISqlDataSourceWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ISqlDataSourceWizardState>;
                queryControl: ko.Observable<Internal.ISqlQueryControl>;
                runQueryBuilderBtnText: ko.PureComputed<any>;
                popupQueryBuilder: Wizard.Internal.QueryBuilderPopup;
                queryTypeItems: string[];
                selectedQueryType: ko.Observable<string>;
            }
            function _registerConfigureQueryPage(factory: PageFactory, dataSourceWizardOptions: _DataSourceWizardOptions): void;
            class ConfigureQueryParametersPage extends WizardPageBase<ISqlDataSourceWizardState, ISqlDataSourceWizardState> {
                private parametersConverter;
                private _requestWrapper;
                private _sqlDataSourceWrapper;
                private _isParametersValid;
                constructor(parametersConverter: Internal.IParametersViewModelConverter, _requestWrapper: QueryBuilder.Utils.RequestWrapper);
                canNext(): boolean;
                canFinish(): boolean;
                getParameters(): any[];
                initialize(data: ISqlDataSourceWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ISqlDataSourceWizardState>;
                removeButtonTitle: any;
                parametersEditorOptions: Analytics.Widgets.Internal.ICollectionEditorOptions;
            }
            function _registerConfigureParametersPage(factory: PageFactory, requestWrapper?: QueryBuilder.Utils.RequestWrapper, parametersConverter?: Internal.IParametersViewModelConverter): void;
            class MultiQueryConfigurePage extends WizardPageBase<ISqlDataSourceWizardState, ISqlDataSourceWizardState> {
                private _options;
                private _callbacks;
                private _selectedPath;
                private _connection;
                private _itemsProvider;
                private _customQueries;
                private _checkedQueries;
                private _sqlTextProvider;
                private _sqlDataSourceWrapper;
                private _dataSource;
                private _dataConnection;
                private _addQueryAlgorithm;
                private _addQueryFromTables;
                private _addQueryFromStoredProcedures;
                private _addQueryFromCustomQueries;
                private _getItemsPromise;
                private _resetDataSourceResult;
                private _setQueryCore;
                static _pushQuery(newQuery: Data.Utils.ISqlQueryViewModel, node: Analytics.Wizard.Internal.TreeLeafNode, queries: ko.ObservableArray<Data.Utils.ISqlQueryViewModel>): void;
                static _removeQuery(queries: ko.ObservableArray<Data.Utils.ISqlQueryViewModel>, node: any): void;
                constructor(_options: _MultiQueryDataSourceWizardOptions);
                canNext(): boolean;
                canFinish(): boolean;
                _showStatementPopup: (query: any) => void;
                _AddQueryWithBuilder(): void;
                _runQueryBuilder(): void;
                _loadPanelViewModel(element: HTMLElement): {
                    animation: {
                        show: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                        hide: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                    };
                    deferRendering: boolean;
                    message: any;
                    visible: ko.Observable<boolean>;
                    shading: boolean;
                    shadingColor: string;
                    position: {
                        of: JQuery;
                    };
                    container: JQuery;
                };
                _setTableQuery(query: Data.TableQuery, isInProcess?: ko.Observable<boolean>): JQueryPromise<QueryBuilder.Utils.ISelectStatementResponse>;
                _setCustomSqlQuery(query: Data.CustomSqlQuery): void;
                _showQbCallBack: (name?: any, isCustomQuery?: boolean) => void;
                commit(): JQueryPromise<ISqlDataSourceWizardState>;
                initialize(state: ISqlDataSourceWizardState): JQueryPromise<Data.Utils.ISqlQueryViewModel>;
                _popupSelectStatement: {
                    isVisible: ko.Observable<boolean>;
                    title: () => any;
                    query: any;
                    data: ko.Observable<any>;
                    okButtonText: () => any;
                    okButtonHandler: (e: any) => void;
                    aceOptions: {
                        showLineNumbers: boolean;
                        showPrintMargin: boolean;
                        enableBasicAutocompletion: boolean;
                        enableLiveAutocompletion: boolean;
                        readOnly: boolean;
                        highlightSelectedWord: boolean;
                        showGutter: boolean;
                        highlightActiveLine: boolean;
                    };
                    aceAvailable: boolean;
                    additionalOptions: {
                        onChange: (session: any) => void;
                        onValueChange: (editor: any) => void;
                        changeTimeout: number;
                        overrideEditorFocus: boolean;
                        setUseWrapMode: boolean;
                    };
                    languageHelper: {
                        getLanguageMode: () => string;
                        createCompleters: () => any[];
                    };
                    closest(element: HTMLElement, parentSelector: string): JQuery;
                };
                _customResetOptions: () => any;
                _queryEditIndex: ko.Observable<number>;
                disableCustomSql: boolean;
                _scrollViewHeight: string;
                _getItemsAfterCheck: (node: Analytics.Wizard.Internal.TreeNode) => any;
                _fieldListModel: ko.Observable<Analytics.Widgets.Internal.ITreeListOptions>;
                _popupQueryBuilder: Analytics.Wizard.Internal.QueryBuilderPopup;
                _customizeDBSchemaTreeListActions: (item: Analytics.Utils.IDataMemberInfo, actions: Analytics.Utils.IAction[]) => void;
                _hasParametersToEdit: ko.Computed<boolean>;
                _isDataLoadingInProcess: ko.Observable<boolean>;
            }
            function _registerMultiQueryConfigurePage(factory: PageFactory, wizardOptions: _MultiQueryDataSourceWizardOptions): void;
            function _canEditQueryParameters(query: Data.Utils.ISqlQueryViewModel, customQueries: Data.Utils.ISqlQueryViewModel[]): boolean;
            class MultiQueryConfigureParametersPage extends WizardPageBase<ISqlDataSourceWizardState, ISqlDataSourceWizardState> {
                private parametersConverter;
                private _requestWrapper;
                private _sqlDataSourceWrapper;
                private _selectedPath;
                private _isParametersValid;
                private _rootItems;
                private _createNewParameter;
                canNext(): boolean;
                canFinish(): boolean;
                constructor(parametersConverter: Internal.IParametersViewModelConverter, _requestWrapper: QueryBuilder.Utils.RequestWrapper);
                _getParameters(): any;
                initialize(state: ISqlDataSourceWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ISqlDataSourceWizardState>;
                _scrollViewHeight: string;
                _fieldListModel: ko.Observable<Analytics.Widgets.Internal.ITreeListOptions>;
                _removeButtonTitle: any;
                _parametersEditorOptions: Analytics.Widgets.Internal.ICollectionEditorOptions;
            }
            function _registerMultiQueryConfigureParametersPage(factory: PageFactory, requestWrapper?: QueryBuilder.Utils.RequestWrapper, parametersConverter?: Internal.IParametersViewModelConverter): void;
            class ConfigureMasterDetailRelationshipsPage extends WizardPageBase<ISqlDataSourceWizardState, ISqlDataSourceWizardState> {
                private _sqlDataSourceResultSchema;
                dispose(): void;
                private _relations;
                private _resultSet;
                private relationsSubscription;
                private _sqlDataSourceWrapper;
                private _updateRelations;
                constructor(_sqlDataSourceResultSchema: (dataSource: Data.SqlDataSource, queryName?: string, relationsEditing?: boolean) => JQueryPromise<{
                    resultSchemaJSON: string;
                    connectionParameters?: string;
                }>);
                canNext(): boolean;
                canFinish(): boolean;
                private _getResultSet;
                initialize(state: ISqlDataSourceWizardState): JQueryPromise<Data.ResultSet>;
                commit(): JQueryPromise<ISqlDataSourceWizardState>;
                _customResetOptions: () => any;
                _relationsEditor: ko.Observable<QueryBuilder.Widgets.Internal.MasterDetailEditor>;
            }
            function _registerConfigureMasterDetailRelationshipsPage(factory: PageFactory, sqlDataSourceResultSchema: (dataSource: Data.SqlDataSource, queryName?: string, relationsEditing?: boolean) => JQueryPromise<{
                resultSchemaJSON: string;
                connectionParameters?: string;
            }>): void;
            class PageFactory {
                registerMetadata<T extends IWizardPage>(pageId: string, metadata: IWizardPageMetadata<T>): void;
                getMetadata(pageId: any): IWizardPageMetadata<IWizardPage>;
                unregisterMetadata(pageId: string): void;
                reset(): void;
                metadata: {
                    [key: string]: IWizardPageMetadata<IWizardPage>;
                };
            }
            class StateManager {
                private globalState;
                private pageFactory;
                private defaultState;
                private _getPageState;
                constructor(globalState: any, pageFactory: PageFactory);
                setPageState(pageId: string, data: any): void;
                getPageState(pageId: string): any;
                resetPageState(pageId: string): void;
                getCurrentState(): any;
                reset(): void;
            }
            class PageIterator<T = any> extends Utils.Disposable {
                pageFactory: PageFactory;
                stateManager: StateManager;
                private _onResetPage;
                dispose(): void;
                private _pages;
                private _currentIndex;
                private __resetPages;
                private _nextPage;
                private _getNextExistingPage;
                _resetPages(): void;
                private _getNextNewPage;
                constructor(pageFactory: PageFactory, stateManager: StateManager, _onResetPage?: (page: _WrappedWizardPage) => void);
                _getStartPage(pageId?: string): _WrappedWizardPage;
                _getNextPage(): JQueryPromise<_WrappedWizardPage>;
                _getPreviousPage(): JQueryPromise<_WrappedWizardPage>;
                _goToPage(pageId: string): JQueryPromise<_WrappedWizardPage>;
                _getCurrentPage(): _WrappedWizardPage;
                _getCurrentState(): T;
                getNextPageId(pageId?: string): string;
            }
            interface IWizardEventArgs<Sender> {
                wizard: Sender;
            }
            interface IWizardPageEventArgs<Sender> extends IWizardEventArgs<Sender> {
                page: IWizardPage;
                pageId: string;
            }
            interface IBeforeWizardPageInitializeEventArgs<Sender> extends IWizardPageEventArgs<Sender>, IBeforeWizardInitializeEventArgs<Sender> {
            }
            interface IBeforeWizardInitializeEventArgs<Sender> extends IWizardEventArgs<Sender> {
                state: any;
            }
            interface IBeforeWizardFinishEventArgs {
                state: any;
                wizardModel?: any;
            }
            interface IAfterWizardFinishEventArgs {
                state: any;
                wizardResult?: any;
            }
            interface IWizardEvents<Sender> {
                "afterInitialize": IWizardEventArgs<Sender>;
                "beforeInitialize": IBeforeWizardInitializeEventArgs<Sender>;
                "beforeStart": IWizardEventArgs<Sender>;
                "beforePageInitialize": IBeforeWizardPageInitializeEventArgs<Sender>;
                "afterPageInitialize": IWizardPageEventArgs<Sender>;
                "beforeFinish": IBeforeWizardFinishEventArgs;
                "afterFinish": IAfterWizardFinishEventArgs;
            }
            class BaseWizard extends Utils.Disposable {
                pageFactory: PageFactory;
                static __loadingStateFunctionName: string;
                static __nextActionFunctionName: string;
                stateManager: StateManager;
                iterator: PageIterator;
                events: Utils.EventManager<BaseWizard, IWizardEvents<BaseWizard>>;
                private _finishCallback;
                protected _createLoadingState(page: IWizardPage): void;
                protected _createNextAction(page: IWizardPage): void;
                private _loadingTimeout;
                protected _loadingState(active: any): void;
                protected _callBeforeFinishHandler(state: any, wizardModel?: any): void;
                protected _callAfterFinishHandler(state: any, result: any): void;
                onFinish(): void;
                constructor(pageFactory: PageFactory, finishCallback?: (model: IDataSourceWizardState) => JQueryPromise<boolean>);
                initialize(state?: any, createIterator?: (pageFactory: PageFactory, stateManager: StateManager) => PageIterator): void;
                isFirstPage(): boolean;
                canNext(): boolean;
                canFinish(): boolean;
                _initPage(page: _WrappedWizardPage): JQueryPromise<any>;
                start(): void;
                canRunWizard(): boolean;
                nextAction(): void;
                previousAction(): void;
                goToPage(pageId: string): void;
                finishAction(): void;
                isLoading: ko.Observable<boolean>;
                _currentPage: ko.Observable<_WrappedWizardPage>;
                isVisible: ko.Observable<boolean>;
            }
            interface IWizardPageSectionMetadata<T extends IWizardPage> extends IWizardPageMetadata<T> {
                position?: number;
                disabledText?: string;
                recreate?: boolean;
                onChange?: () => void;
            }
            module Internal {
                class WrappedWizardPageSection extends _WrappedWizardPage {
                    pageId: string;
                    page: IWizardPage;
                    onChange: (callback: () => void) => void;
                    constructor(pageId: string, page: IWizardPage, metadata: IWizardPageSectionMetadata<IWizardPage>);
                }
                class WizardPageSection {
                    pageId: string;
                    metadata: IWizardPageSectionMetadata<IWizardPage>;
                    resetPage(): void;
                    setPage(page: _WrappedWizardPage): void;
                    constructor(pageId: string, metadata: IWizardPageSectionMetadata<IWizardPage>);
                    page: ko.Observable<_WrappedWizardPage>;
                }
                class WizardPageSectionIterator {
                    pageFactory: WizardPageSectionFactory;
                    stateManager: StateManager;
                    private _resetPageCallback;
                    private _pagesIds;
                    private _pages;
                    private _resetPages;
                    private _tryResetPageByMetadata;
                    private _resetPage;
                    private _createNewPage;
                    private _getPage;
                    private _getNextPage;
                    private _getPageIndex;
                    resetNextPages(pageId: string): void;
                    constructor(pageFactory: WizardPageSectionFactory, stateManager: StateManager, _resetPageCallback: (pageId: string) => void);
                    getStartPage(): WrappedWizardPageSection;
                    getNextPage(currentPageId: string): JQueryPromise<WrappedWizardPageSection[]>;
                    getCurrentState(): any;
                    getNextPageId(pageId?: string): string | string[];
                }
                class WizardPageSectionFactory extends Wizard.PageFactory {
                    registerMetadata<T extends IWizardPage>(pageId: string, metadata: IWizardPageSectionMetadata<T>): void;
                    metadata: {
                        [key: string]: IWizardPageSectionMetadata<IWizardPage>;
                    };
                }
                class WizardPageProcessor extends Utils.Disposable {
                    pageFactory: WizardPageSectionFactory;
                    dispose(): void;
                    static __loadingStateFunctionName: string;
                    stateManager: StateManager;
                    iterator: WizardPageSectionIterator;
                    events: Utils.EventManager<WizardPageProcessor, IWizardEvents<WizardPageProcessor>>;
                    protected _createLoadingState(page: IWizardPage): void;
                    protected _createNextAction(page: IWizardPage): void;
                    private _loadingTimeout;
                    private _changeTimeout;
                    protected _loadingState(active: any): void;
                    protected _extendedNextAction(): void;
                    constructor(pageFactory: WizardPageSectionFactory, _loadingState?: (boolean: any) => void, _nextAction?: () => void);
                    private _resetPageById;
                    initialize(state: IDataSourceWizardState, createIterator?: (pageFactory: WizardPageSectionFactory, stateManager: StateManager) => WizardPageSectionIterator): void;
                    private _canNext;
                    private _canFinish;
                    private _initPage;
                    getPageById(pageId: any): WizardPageSection;
                    start(): void;
                    finishAction(): JQueryPromise<{}>;
                    private _nextAction;
                    sections: WizardPageSection[];
                    isLoading: ko.Observable<boolean>;
                }
            }
            class PopupWizard extends BaseWizard {
                static _getLoadPanelViewModel(element: HTMLElement, observableVisible: ko.Observable<boolean>): {
                    animation: {
                        show: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                        hide: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                    };
                    deferRendering: boolean;
                    message: any;
                    visible: ko.Observable<boolean>;
                    shading: boolean;
                    shadingColor: string;
                    position: {
                        of: JQuery;
                    };
                    container: JQuery;
                };
                constructor(pageFactory: any, finishCallback?: any);
                start(): void;
                height: ko.Observable<number>;
                width: ko.Observable<number>;
                _extendCssClass: string;
                _container: (el: HTMLElement) => JQuery;
                nextButton: {
                    text: any;
                    disabled: ko.Computed<boolean>;
                    onClick: () => void;
                };
                cancelButton: {
                    text: any;
                    onClick: () => any;
                };
                previousButton: {
                    text: any;
                    disabled: ko.Computed<boolean>;
                    onClick: () => void;
                };
                finishButton: {
                    text: any;
                    disabled: ko.Computed<boolean>;
                    onClick: () => void;
                };
                _wizardPopupPosition(element: HTMLElement): {
                    of: JQuery;
                };
                _loadPanelViewModel(element: HTMLElement): {
                    animation: {
                        show: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                        hide: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                    };
                    deferRendering: boolean;
                    message: any;
                    visible: ko.Observable<boolean>;
                    shading: boolean;
                    shadingColor: string;
                    position: {
                        of: JQuery;
                    };
                    container: JQuery;
                };
                _getLoadPanelViewModel(element: HTMLElement, observableVisible: ko.Observable<boolean>): {
                    animation: {
                        show: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                        hide: {
                            type: string;
                            from: number;
                            to: number;
                            duration: number;
                        };
                    };
                    deferRendering: boolean;
                    message: any;
                    visible: ko.Observable<boolean>;
                    shading: boolean;
                    shadingColor: string;
                    position: {
                        of: JQuery;
                    };
                    container: JQuery;
                };
                _titleTemplate: string;
                title: string;
            }
            class _DataSourceWizardOptionsBase<T extends Internal.IDataSourceWizardCallbacks> {
                readonly jsonDataSourceAvailable: boolean;
                readonly sqlDataSourceAvailable: boolean;
                connectionStrings: Analytics.Wizard.IDataSourceWizardConnectionStrings;
                callbacks: T;
                rtl: boolean;
                requestWrapper: QueryBuilder.Utils.RequestWrapper;
                disableCustomSql: boolean;
                wizardSettings: IDataSourceWizardSettings;
                queryName: string;
                canCreateNewJsonDataSource: boolean;
            }
            class _DataSourceWizardOptions extends _DataSourceWizardOptionsBase<Internal.IDataSourceWizardCallbacks> {
            }
            interface IDataSourceWizardSettings {
                enableJsonDataSource?: boolean;
                enableSqlDataSource?: boolean;
            }
            class DataSourceWizardSettings implements IDataSourceWizardSettings {
                createDefault(settings?: IDataSourceWizardSettings): IDataSourceWizardSettings;
                enableJsonDataSource?: boolean;
                enableSqlDataSource?: boolean;
            }
            interface IRetrieveQuerySqlCallback {
                (query: Data.TableQuery, isInProcess: ko.Observable<boolean>): JQueryPromise<QueryBuilder.Utils.ISelectStatementResponse>;
            }
            class DataSourceWizardPageIterator extends PageIterator {
                private _dataSourceWizardOptions;
                constructor(pageFactory: PageFactory, stateManager: StateManager, _dataSourceWizardOptions: _DataSourceWizardOptions);
                getNextPageId(pageId: string): any;
            }
            class DataSourceWizard extends PopupWizard {
                private _wizardOptions;
                constructor(pageFactory: any, _wizardOptions: _DataSourceWizardOptions);
                initialize(state: IDataSourceWizardState, createIterator?: (pageFactory: PageFactory, stateManager: StateManager) => PageIterator): void;
                canRunWizard(): boolean;
                _extendCssClass: string;
                title: any;
            }
            function _registerSqlDataSourceWizardPages(factory: PageFactory, dataSourceWizardOptions: _DataSourceWizardOptions): PageFactory;
            function _createSqlDataSourceWizard(factory: PageFactory, dataSourceWizardOptions: _DataSourceWizardOptions): DataSourceWizard;
            class _MultiQueryDataSourceWizardOptions extends _DataSourceWizardOptionsBase<Internal.IMultiQueryDataSourceWizardCallbacks> {
            }
            class MultiQueryDataSourceWizard extends PopupWizard {
                private _wizardOptions;
                constructor(pageFactory: any, _wizardOptions: _MultiQueryDataSourceWizardOptions);
                canRunWizard(): boolean;
                initialize(state: IDataSourceWizardState, createIterator?: (pageFactory: PageFactory, stateManager: StateManager) => PageIterator): void;
                title: any;
                _extendCssClass: string;
            }
            class MultiQueryDataSourceWizardPageIterator<T extends IDataSourceWizardState = IDataSourceWizardState> extends PageIterator<T> {
                private _wizardOptions;
                constructor(pagesFactory: PageFactory, stateManager: StateManager, _wizardOptions: _MultiQueryDataSourceWizardOptions);
                getNextPageId(pageId?: string): string;
            }
            function _registerMultiQueryDataSourcePages(factory: PageFactory, dataSourceWizardOptions: _MultiQueryDataSourceWizardOptions): PageFactory;
            function _createMultiQueryDataSourceWizard(factory: PageFactory, dataSourceWizardOptions: _MultiQueryDataSourceWizardOptions): MultiQueryDataSourceWizard;
            interface IFullscreenWizardPageMetadata<T extends Analytics.Wizard.IWizardPage> extends Analytics.Wizard.IWizardPageMetadata<T> {
                navigationPanelText?: string;
            }
            class FullscreenWizardPageFactory extends Analytics.Wizard.PageFactory {
                registerMetadata<T extends Analytics.Wizard.IWizardPage>(pageId: string, metadata: IFullscreenWizardPageMetadata<T>): void;
                getMetadata(key: string): IFullscreenWizardPageMetadata<Analytics.Wizard.IWizardPage>;
                metadata: {
                    [key: string]: IFullscreenWizardPageMetadata<Analytics.Wizard.IWizardPage>;
                };
            }
            enum WizardSectionPosition {
                Left = 1,
                TopLeft = 2,
                BottomLeft = 3,
                Right = 4,
                TopRight = 5,
                BottomRight = 6,
                Top = 7,
                Bottom = 8
            }
            interface IWizardPageStyle {
                top?: any;
                bottom?: any;
                left?: any;
                right?: any;
                width?: any;
                height?: any;
                display?: any;
            }
            interface IBeforeWizardSectionInitializeEventArgs<Sender> extends IWizardSectionEventArgs<Sender> {
                state: any;
            }
            interface IWizardSectionEventArgs<Sender> {
                section: IWizardPage;
                sectionId: string;
                page: Sender;
            }
            interface IWizardFullscreenPageEvents<Sender> {
                "beforeSectionInitialize": IBeforeWizardSectionInitializeEventArgs<Sender>;
                "afterSectionInitialize": IWizardSectionEventArgs<Sender>;
            }
            class FullscreenWizardPage extends Analytics.Wizard.WizardPageBase {
                dispose(): void;
                private _patchOnChange;
                private _getPageStyle;
                private _sectionsToUnregister;
                private _sectionsToRegister;
                private _sectionPositions;
                private _applyCustomizations;
                protected _setSectionPosition(pageId: string, position?: WizardSectionPosition): void;
                constructor();
                registerSections(): void;
                canNext(): boolean;
                canFinish(): boolean;
                setSectionPosition(sectionId: string, position?: WizardSectionPosition): void;
                registerSection(sectionId: string, metadata: IWizardPageMetadata<IWizardPage>): void;
                unregisterSection(sectionId: string): void;
                _loadPanelViewModel(element: HTMLElement): boolean;
                getNextSectionId(sectionId: string): any;
                initialize(state: any): JQueryPromise<any>;
                _beforeStart(): void;
                commit(): JQueryPromise<any>;
                _getPageDescription(index: number, page: Analytics.Wizard.Internal.WizardPageSection): string;
                _showPageDescription(): boolean;
                _initInProgress: ko.Observable<boolean>;
                _defaultMargin: number;
                _parentMarginOffset: number;
                _sections: Analytics.Wizard.Internal.WizardPageSection[];
                _pageCss: {
                    [key: string]: ko.Observable<IWizardPageStyle>;
                };
                _factory: DevExpress.Analytics.Wizard.Internal.WizardPageSectionFactory;
                _stateManager: Analytics.Wizard.StateManager;
                _sectionsProcessor: Analytics.Wizard.Internal.WizardPageProcessor;
                events: Utils.EventManager<FullscreenWizardPage, IWizardFullscreenPageEvents<FullscreenWizardPage>>;
            }
            class SpecifyJsonDataSourceSettingsPage extends FullscreenWizardPage {
                private _dataSourceWizardOptions;
                constructor(_dataSourceWizardOptions: _DataSourceWizardOptions);
                registerSections(): void;
                getNextSectionId(sectionId: string): string;
            }
            function _registerSpecifyJsonDataSourceSettingsPage(factory: FullscreenWizardPageFactory, dataSourceWizardOptions: _DataSourceWizardOptions): void;
            class SpecifySqlDataSourceSettingsPage extends FullscreenWizardPage {
                private _dataSourceWizardOptions;
                constructor(_dataSourceWizardOptions: _MultiQueryDataSourceWizardOptions);
                getNextSectionId(sectionId: string): string | any[];
                registerSections(): void;
            }
            function _registerSpecifySqlDataSourceSettingsPage(factory: FullscreenWizardPageFactory, dataSourceWizardOptions: _MultiQueryDataSourceWizardOptions): void;
            class FullscreenWizard extends Analytics.Wizard.PopupWizard {
                private _onCloseCallback;
                constructor(pageFactory: FullscreenWizardPageFactory, finishCallback?: any);
                _onClose(callback: any): void;
                onFinish(): void;
                _initPage(page: any): JQueryPromise<any>;
                _onResetPage(page: Analytics.Wizard._WrappedWizardPage): void;
                start(finishCallback?: (model: any) => JQueryPromise<boolean>): void;
                _pageDescription(): string;
                _description(): string;
                navigationPanel: ko.Observable<WizardNavigationPanel>;
                _steps: FullscreenWizardPage[];
                _extendCssClass: string;
                pageFactory: FullscreenWizardPageFactory;
            }
            interface IWizardNavigationStep {
                pageIds: string[];
                currentPageId: string;
                clickAction: () => void;
                text: string;
                stepIndex: number;
                isActive: ko.Observable<boolean> | ko.Computed<boolean>;
                disabled: ko.Observable<boolean> | ko.Computed<boolean>;
                visible: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            class WizardNavigationPanel extends Analytics.Utils.Disposable {
                constructor(wizard: FullscreenWizard);
                resetAll(): void;
                _reset(pageId: string): void;
                _resetNextPages(pageId: string): void;
                _setStepVisible(currentPageIndex: number): void;
                _steps: Array<IWizardNavigationStep>;
                isVisible: ko.Computed<boolean>;
            }
            class FullscreenDataSourceWizard extends FullscreenWizard {
                private _dataSourceWizardOptions;
                constructor(factory: any, _dataSourceWizardOptions: _DataSourceWizardOptions);
                initialize(state: IDataSourceWizardState, createIterator?: (pageFactory: PageFactory, stateManager: StateManager) => PageIterator): void;
                canRunWizard(): boolean;
                _description(): any;
            }
            class FullscreenDataSourceWizardPageIterator extends PageIterator {
                private _dataSourceOptions;
                constructor(factory: any, stateManager: any, _dataSourceOptions: _DataSourceWizardOptions, onResetPage: any);
                getNextPageId(pageId?: string): string;
            }
            function _createDataSourceFullscreenWizard(dataSourceWizardOptions: _MultiQueryDataSourceWizardOptions): FullscreenDataSourceWizard;
            interface IConnectionStringDefinition {
                name: string;
                description?: string;
            }
            interface IDataSourceWizardConnectionStrings {
                sql: ko.ObservableArray<IConnectionStringDefinition>;
                json?: ko.ObservableArray<IConnectionStringDefinition>;
            }
            module Legacy {
                class WizardPage<TWizardModel> {
                    constructor(wizard: WizardViewModel<TWizardModel>, template?: string, title?: string, description?: string);
                    template: string;
                    title: string;
                    description: string;
                    wizard: WizardViewModel<TWizardModel>;
                    isVisible: boolean;
                    actionCancel: Internal.WizardAction;
                    actionPrevious: Internal.WizardAction;
                    actionNext: Internal.WizardAction;
                    actionFinish: Internal.WizardAction;
                    _begin(data: TWizardModel): void;
                    beginAsync(data: TWizardModel): JQueryPromise<any>;
                    commit(data: TWizardModel): void;
                    reset(): void;
                }
                class WizardViewModel<TWizardModel> {
                    static WIZARD_DEFAULT_WIDTH: string;
                    static WIZARD_DEFAULT_HEIGHT: string;
                    static WIZARD_DEFAULT_SCROLLVIEW_HEIGHT: string;
                    protected _data: TWizardModel;
                    private _defaultWizardPage;
                    private _finishCallback;
                    private _pageToken;
                    private _pageIsFirst;
                    private _currentStep;
                    protected _goToPage(task: JQueryPromise<IPageInfo<TWizardModel>>): void;
                    constructor();
                    width: ko.Observable<string>;
                    height: ko.Observable<string>;
                    title: string;
                    headerTemplate: string;
                    extendCssClass: string;
                    steps: WizardPage<TWizardModel>[];
                    renderedSteps: ko.ObservableArray<WizardPage<TWizardModel>>;
                    isVisible: ko.Observable<boolean>;
                    indicatorVisible: ko.Observable<boolean>;
                    titleTemplate: string;
                    dispatcher: IPageDispatcher<TWizardModel>;
                    readonly currentStep: any;
                    container: (element: HTMLElement) => JQuery;
                    loadPanelViewModel(element: HTMLElement): {
                        animation: {
                            show: {
                                type: string;
                                from: number;
                                to: number;
                                duration: number;
                            };
                            hide: {
                                type: string;
                                from: number;
                                to: number;
                                duration: number;
                            };
                        };
                        deferRendering: boolean;
                        message: any;
                        visible: ko.Observable<boolean>;
                        shading: boolean;
                        shadingColor: string;
                        position: {
                            of: JQuery;
                        };
                        container: JQuery;
                    };
                    getLoadPanelViewModel(element: HTMLElement, observableVisible: ko.Observable<boolean>): {
                        animation: {
                            show: {
                                type: string;
                                from: number;
                                to: number;
                                duration: number;
                            };
                            hide: {
                                type: string;
                                from: number;
                                to: number;
                                duration: number;
                            };
                        };
                        deferRendering: boolean;
                        message: any;
                        visible: ko.Observable<boolean>;
                        shading: boolean;
                        shadingColor: string;
                        position: {
                            of: JQuery;
                        };
                        container: JQuery;
                    };
                    wizardPopupPosition(element: HTMLElement): {
                        of: JQuery;
                    };
                    goToNext(): void;
                    goToPrevious(): void;
                    isPreviousButtonDisabled: ko.PureComputed<any>;
                    cancel(): void;
                    start(data: TWizardModel, finishCallback?: (model: TWizardModel) => JQueryPromise<boolean>): void;
                    finish(): void;
                    resetState(): void;
                    static chainCallbacks<T>(first: (data: T) => JQueryPromise<boolean>, second: (data: T) => JQueryPromise<boolean>): (data: T) => JQueryPromise<boolean>;
                }
                class CommonParametersPage<T> extends WizardPage<T> {
                    private _validation;
                    getParameters(): any[];
                    validateParameters(): void;
                    reset(): void;
                    commit(data: T): void;
                }
                interface ITypeItem {
                    text: string;
                    imageClassName: string;
                    imageTemplateName: string;
                    type: number;
                }
                enum DataSourceType {
                    NoData = 0,
                    Sql = 1,
                    Json = 2
                }
                class TypeItem implements ITypeItem {
                    constructor(textDefault: string, textID: string, imageClassName: string, imageTemplateName: string, type: number);
                    text: string;
                    imageClassName: string;
                    imageTemplateName: string;
                    type: number;
                }
                class ChooseDataSourceTypePage<T extends IDataSourceWizardModel> extends WizardPage<T> {
                    private _isActivated;
                    constructor(wizard: WizardViewModel<T>, wizardSettings?: IDataSourceWizardSettings);
                    template: string;
                    description: any;
                    typeItems: ITypeItem[];
                    selectedItem: ko.Observable<ITypeItem>;
                    extendCssClass: (rightPath: string) => string;
                    itemClick: (item: ITypeItem) => void;
                    IsSelected: (item: ITypeItem) => boolean;
                    _begin(data: T): void;
                    commit(data: T): void;
                    reset(): void;
                }
                class MasterDetailRelationsPage extends WizardPage<MultiQueryDataSourceWizardModel> {
                    private _sqlDataSourceResultSchema;
                    private _relations;
                    private _resultSet;
                    private relationsSubscription;
                    constructor(wizard: WizardViewModel<MultiQueryDataSourceWizardModel>, sqlDataSourceResultSchema: (dataSource: Data.SqlDataSource) => JQueryPromise<{
                        resultSchemaJSON: string;
                        connectionParameters?: string;
                    }>);
                    template: string;
                    description: any;
                    relationsEditor: ko.Observable<QueryBuilder.Widgets.Internal.MasterDetailEditor>;
                    private _getResultSet;
                    beginAsync(data: MultiQueryDataSourceWizardModel): JQueryPromise<any>;
                    customResetOptions: () => any;
                    commit(data: MultiQueryDataSourceWizardModel): void;
                }
                class MultiQueryDataSourceWizard extends WizardViewModel<MultiQueryDataSourceWizardModel> {
                    connectionStrings: IDataSourceWizardConnectionStrings;
                    constructor(connectionStrings: IDataSourceWizardConnectionStrings, wizardSettings?: IDataSourceWizardSettings, callbacks?: Internal.IMultiQueryDataSourceWizardCallbacks, disableCustomSql?: boolean, rtl?: boolean);
                    start(wizardModel?: MultiQueryDataSourceWizardModel, finishCallback?: (model: MultiQueryDataSourceWizardModel) => JQueryPromise<boolean>): void;
                    height: ko.Observable<string>;
                    title: any;
                    extendCssClass: string;
                    container: (element: HTMLElement) => JQuery;
                    finishCallback: (any: any) => JQueryPromise<any>;
                    wizardModel: MultiQueryDataSourceWizardModel;
                }
                class MultiQueryConfigurePage extends WizardPage<MultiQueryDataSourceWizardModel> {
                    private _callbacks;
                    private _selectedPath;
                    private _connection;
                    private _itemsProvider;
                    private _customQueries;
                    private _checkedQueries;
                    private _data;
                    private _sqlTextProvider;
                    private _dataSourceClone;
                    private _dataSource;
                    private _dataConnection;
                    private _addQueryAlgorithm;
                    private _addQueryFromTables;
                    private _addQueryFromStoredProcedures;
                    private _addQueryFromCustomQueries;
                    private _getItemsPromise;
                    private _resetDataSourceResult;
                    private _setQueryCore;
                    static pushQuery(newQuery: Data.Utils.ISqlQueryViewModel, node: Internal.TreeLeafNode, queries: ko.ObservableArray<Data.Utils.ISqlQueryViewModel>): void;
                    static removeQuery(queries: ko.ObservableArray<Data.Utils.ISqlQueryViewModel>, node: any): void;
                    constructor(wizard: WizardViewModel<MultiQueryDataSourceWizardModel>, callbacks?: Internal.IDataSourceWizardCallbacks, disableCustomSql?: boolean, rtl?: boolean);
                    template: string;
                    description: any;
                    scrollViewHeight: string;
                    getItemsAfterCheck: (node: Internal.TreeNode) => any;
                    isTablesGenerateColumnsCallBack: ko.ObservableArray<JQueryPromise<any>>;
                    fieldListModel: ko.Observable<Analytics.Widgets.Internal.ITreeListOptions>;
                    popupQueryBuilder: Internal.QueryBuilderPopup;
                    customizeDBSchemaTreeListActions: (item: Analytics.Utils.IDataMemberInfo, actions: Analytics.Utils.IAction[]) => void;
                    popupSelectStatment: {
                        isVisible: ko.Observable<boolean>;
                        title: () => any;
                        query: any;
                        data: ko.Observable<any>;
                        okButtonText: () => any;
                        okButtonHandler: (e: any) => void;
                        aceOptions: {
                            showLineNumbers: boolean;
                            showPrintMargin: boolean;
                            enableBasicAutocompletion: boolean;
                            enableLiveAutocompletion: boolean;
                            readOnly: boolean;
                            highlightSelectedWord: boolean;
                            showGutter: boolean;
                            highlightActiveLine: boolean;
                        };
                        aceAvailable: boolean;
                        additionalOptions: {
                            onChange: (session: any) => void;
                            onValueChange: (editor: any) => void;
                            changeTimeout: number;
                            overrideEditorFocus: boolean;
                            setUseWrapMode: boolean;
                        };
                        languageHelper: {
                            getLanguageMode: () => string;
                            createCompleters: () => any[];
                        };
                        closest(element: HTMLElement, parentSelector: string): JQuery;
                    };
                    showStatementPopup: (query: any) => void;
                    disableCustomSql: boolean;
                    dataSourceClone(): Data.SqlDataSource;
                    AddQueryWithBuilder(): void;
                    runQueryBuilder(): void;
                    hasParametersToEdit: ko.Computed<boolean>;
                    isDataLoadingInProcess: ko.Observable<boolean>;
                    loadPanelViewModel(element: HTMLElement): {
                        animation: {
                            show: {
                                type: string;
                                from: number;
                                to: number;
                                duration: number;
                            };
                            hide: {
                                type: string;
                                from: number;
                                to: number;
                                duration: number;
                            };
                        };
                        deferRendering: boolean;
                        message: any;
                        visible: ko.Observable<boolean>;
                        shading: boolean;
                        shadingColor: string;
                        position: {
                            of: JQuery;
                        };
                        container: JQuery;
                    };
                    setTableQuery(query: Data.TableQuery, isInProcess?: ko.Observable<boolean>): JQueryPromise<QueryBuilder.Utils.ISelectStatementResponse>;
                    setCustomSqlQuery(query: Data.CustomSqlQuery): void;
                    queryEditIndex: ko.Observable<number>;
                    showQbCallBack: (name?: any, isCustomQuery?: boolean) => void;
                    customResetOptions: () => any;
                    beginAsync(data: MultiQueryDataSourceWizardModel): JQueryPromise<any>;
                    commit(data: MultiQueryDataSourceWizardModel): void;
                }
                class MultiQueryConfigureParametersPage extends CommonParametersPage<MultiQueryDataSourceWizardModel> {
                    private parametersConverter;
                    private _selectedPath;
                    private _rootItems;
                    private _createNewParameter;
                    constructor(wizard: any, parametersConverter?: Internal.IParametersViewModelConverter);
                    template: string;
                    description: any;
                    scrollViewHeight: string;
                    fieldListModel: ko.Observable<Analytics.Widgets.Internal.ITreeListOptions>;
                    _begin(data: MultiQueryDataSourceWizardModel): void;
                    getParameters(): any;
                    commit(data: MultiQueryDataSourceWizardModel): void;
                }
                class MultiQueryDataSourceWizardModel implements IDataSourceWizardModel {
                    requestWrapper: QueryBuilder.Utils.RequestWrapper;
                    private _queryIndex;
                    sqlDataSource: Data.SqlDataSource;
                    jsonDataSource: Data.JsonDataSource;
                    connectionString: ko.Observable<string>;
                    jsonDataSourceConnectionName: ko.Observable<string>;
                    customQueries: ko.ObservableArray<Data.Utils.ISqlQueryViewModel>;
                    constructor(requestWrapper?: QueryBuilder.Utils.RequestWrapper);
                }
                class SelectOptionalConnectionString<T extends IDataSourceWizardModel> extends WizardPage<T> {
                    availableDataSources: ko.Observable<any[]> | ko.Computed<any[]> | ko.ObservableArray<any>;
                    constructor(wizard: WizardViewModel<T>, availableDataSources: ko.Observable<any[]> | ko.Computed<any[]> | ko.ObservableArray<any>, isDataSourceCreationAvailable: ko.Observable<boolean> | ko.Computed<boolean>);
                    template: string;
                    description: any;
                    selectedDataSource: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>;
                    isDataSourceCreationAvailable: ko.Observable<boolean> | ko.Computed<boolean>;
                    dataSourcesListHeight: ko.Computed<number>;
                    dataSourceOperations: {
                        text: any;
                        createNewDataSource: boolean;
                    }[];
                    selectedDataSourceOperation: ko.Observable<{
                        text: any;
                        createNewDataSource: boolean;
                    }>;
                    createNewDataSource: ko.PureComputed<boolean>;
                    _begin(data: T): void;
                    reset(): void;
                    getSelectedDataSource(data?: T): Analytics.Internal.IDataSourceInfo[];
                    readonly createNewDataSourceOperationText: any;
                    readonly existingOperationText: any;
                }
                class JsonSelectConnectionString<T extends IDataSourceWizardModel> extends SelectOptionalConnectionString<T> {
                    constructor(wizard: WizardViewModel<T>, jsonDataConnections: ko.ObservableArray<IConnectionStringDefinition>, isDataSourceCreationAvailable: ko.Observable<boolean> | ko.Computed<boolean>);
                    _begin(data: T): void;
                    commit(data: T): void;
                    getSelectedDataSource(data: T): Analytics.Internal.IDataSourceInfo[];
                }
                class JsonDataSourceFieldsPage<T extends IDataSourceWizardModel> extends WizardPage<T> {
                    private _rootItems;
                    private _fieldListItemsProvider;
                    private _fieldSelectedPath;
                    private _dataSource;
                    private _subscriptions;
                    private _clear;
                    private _createFieldListCallback;
                    private _isDataSourceChanged;
                    private _getSchemaToDataMemberInfo;
                    private _mapJsonNodesToTreelistItems;
                    private _getNodesByPath;
                    private _getInnerItemsByPath;
                    private _beginInternal;
                    private _createTreeNode;
                    private _createLeafTreeNode;
                    private _resetSelectionRecursive;
                    private _mapJsonSchema;
                    constructor(wizard: WizardViewModel<T>);
                    _begin(data: IDataSourceWizardModel): void;
                    beginAsync(data: IDataSourceWizardModel): JQueryPromise<any>;
                    commit(data: IDataSourceWizardModel): void;
                    reset(): void;
                    template: string;
                    rootElementTitle: any;
                    description: any;
                    rootElementList: ko.Observable<Utils.IPathRequest[]>;
                    selectedRootElement: ko.Observable<Utils.IPathRequest>;
                    fieldListModel: Analytics.Widgets.Internal.ITreeListOptions;
                }
                class JsonDataSourceJsonSourcePage<T extends IDataSourceWizardModel> extends WizardPage<T> {
                    private _requestWrapper;
                    private _jsonStringSettings;
                    private _jsonUriSetting;
                    constructor(wizard: WizardViewModel<T>);
                    _begin(data: T): void;
                    reset(): void;
                    commit(data: T): void;
                    grid: Widgets.ObjectProperties;
                    template: string;
                    description: any;
                    jsonSourceTitle: any;
                    sources: Array<IJsonDataSourceType>;
                    _selectedSource: ko.Computed<IJsonDataSourceJsonSourcePageSettings>;
                }
                class ConfigureQueryParametersPage extends CommonParametersPage<DataSourceWizardModel> {
                    private parametersConverter;
                    constructor(wizard: any, parametersConverter?: Internal.IParametersViewModelConverter);
                    template: string;
                    description: any;
                    removeButtonTitle: any;
                    parametersEditorOptions: Analytics.Widgets.Internal.ICollectionEditorOptions;
                    getParameters(): any[];
                    _begin(data: DataSourceWizardModel): void;
                    commit(data: DataSourceWizardModel): void;
                }
                class CreateQueryPage extends WizardPage<DataSourceWizardModel> {
                    static QUERY_TEXT: string;
                    static SP_TEXT: string;
                    private _proceduresList;
                    private _selectStatementControl;
                    private _data;
                    private _dataSource;
                    private _connection;
                    private _wrapWizardIndicator;
                    constructor(wizard: WizardViewModel<DataSourceWizardModel>, callbacks?: Internal.IDataSourceWizardCallbacks, disableCustomSql?: boolean, rtl?: boolean);
                    template: string;
                    description: any;
                    queryTypeItems: string[];
                    selectedQueryType: ko.Observable<string> | ko.Computed<string>;
                    queryControl: ko.Observable<Internal.ISqlQueryControl> | ko.Computed<Internal.ISqlQueryControl>;
                    runQueryBuilderBtnText: ko.PureComputed<any>;
                    popupQueryBuilder: Wizard.Internal.QueryBuilderPopup;
                    runQueryBuilder(): void;
                    localizeQueryType(queryTypeString: string): any;
                    _begin(data: DataSourceWizardModel): void;
                    commit(data: DataSourceWizardModel): void;
                }
                class SelectConnectionString<T extends IDataSourceWizardModel> extends WizardPage<T> {
                    private _showPageForSingleConnectionString;
                    constructor(wizard: WizardViewModel<T>, connectionStrings: ko.ObservableArray<IConnectionStringDefinition>, _showPageForSingleConnectionString?: boolean);
                    template: string;
                    description: any;
                    connectionStrings: ko.ObservableArray<IConnectionStringDefinition>;
                    selectedConnectionString: ko.ObservableArray<IConnectionStringDefinition>;
                    _begin(data: T): void;
                    commit(data: T): void;
                }
                class SqlDataSourceWizard extends WizardViewModel<DataSourceWizardModel> {
                    private _wizardModel;
                    private finishCallback;
                    constructor(connectionStrings: IDataSourceWizardConnectionStrings, wizardSettings?: IDataSourceWizardSettings, callbacks?: Internal.IDataSourceWizardCallbacks, disableCustomSql?: boolean, rtl?: boolean);
                    start(wizardModel?: DataSourceWizardModel, finishCallback?: (model: DataSourceWizardModel) => JQueryPromise<boolean>): void;
                    connectionStrings: IDataSourceWizardConnectionStrings;
                    title: any;
                    extendCssClass: string;
                    container: (el: HTMLElement) => JQuery;
                }
                interface IDataSourceWizardModel {
                    sqlDataSource: Data.SqlDataSource;
                    jsonDataSource?: Data.JsonDataSource;
                    jsonDataSourceConnectionName?: ko.Observable<string> | ko.Computed<string>;
                    dataSourceType?: DataSourceType;
                }
                class DataSourceWizardModel implements IDataSourceWizardModel {
                    private _queryIndex;
                    sqlDataSource: Data.SqlDataSource;
                    jsonDataSource: Data.JsonDataSource;
                    jsonDataSourceConnectionName: ko.Observable<any>;
                    sqlQuery: Data.Utils.ISqlQueryViewModel;
                    constructor(dataSource?: Data.SqlDataSource, queryName?: string);
                    getQueryIndex(): number;
                    dataSourceType: DataSourceType;
                }
            }
            module Internal {
                class JsonStringEditor extends Analytics.Widgets.Editor {
                    constructor(modelPropertyInfo: Analytics.Utils.ISerializationInfo, level: any, parentDisabled: any, textToSearch: any);
                    uploadFile(e: any): void;
                    getUploadTitle(): any;
                    aceEditorHasErrors: ko.Observable<boolean>;
                    aceAvailable: boolean;
                    editorContainer: ko.Observable<any>;
                    _model: ko.Observable<any>;
                    languageHelper: {
                        getLanguageMode: () => string;
                        createCompleters: () => any[];
                    };
                    aceOptions: {
                        showLineNumbers: boolean;
                        highlightActiveLine: boolean;
                        showPrintMargin: boolean;
                        enableBasicAutocompletion: boolean;
                        enableLiveAutocompletion: boolean;
                    };
                    isValid: ko.Computed<any>;
                    additionalOptions: {
                        onChangeAnnotation: (session: any) => void;
                        onBlur: () => void;
                    };
                    jsonStringValidationRules: Array<any>;
                }
            }
            module Internal {
                function getLocalizedValidationErrorMessage(emptyValueErrorMessage: string, localizedPropertyName?: string, subProperty?: string): any;
                interface IJSONSourcePagePropertyDescriptor {
                    value: ko.Observable<any>;
                    displayName: () => string;
                }
                abstract class JsonDataSourceJsonSourcePageSettingsBase extends Utils.Disposable implements IJsonDataSourceJsonSourceValidatable {
                    dispose(): void;
                    protected _validationGroup: any;
                    protected _validationSummary: any;
                    private _onValidationGroupInitialized;
                    private _onValidationGroupDisposing;
                    private _onValidationSummaryInitialized;
                    private _onValidationSummaryDisposing;
                    protected _repaintSummary(): void;
                    abstract _validatorsReady: ko.Observable<boolean> | ko.Computed<boolean>;
                    _validate(): void;
                    constructor();
                    validationGroup: {
                        onInitialized: (args: any) => void;
                        onDisposing: (args: any) => void;
                        validate: () => void;
                    };
                    validationSummary: {
                        onInitialized: (args: any) => void;
                        onDisposing: (args: any) => void;
                    };
                    isValid: ko.Observable<boolean> | ko.Computed<boolean>;
                    grid: Widgets.ObjectProperties;
                }
                class JsonDataSourceJsonSourcePageStringSettings extends JsonDataSourceJsonSourcePageSettingsBase implements IJsonDataSourceJsonSourcePageSettings {
                    onChange(_onChange: () => void): any;
                    _validatorsReady: ko.Observable<boolean>;
                    private _isJsonSourceValid;
                    isEmpty(): boolean;
                    reset(): void;
                    setValue(dataSource: Data.JsonDataSource): void;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    applySettings(jsonDataSource: Data.JsonDataSource): void;
                    constructor();
                    isValid: ko.Observable<boolean> | ko.Computed<boolean>;
                    validationGroup: any;
                    validationSummary: any;
                    stringSource: ko.Observable<string> | ko.Computed<string>;
                    aceEditorHasErrors: ko.Observable<boolean>;
                    grid: Widgets.ObjectProperties;
                    cssClass: {
                        'dxrd-wizard-json-string-source-grid': boolean;
                    };
                }
                class JsonDataSourceJsonSourcePageUriSettings extends JsonDataSourceJsonSourcePageSettingsBase implements IJsonDataSourceJsonSourcePageSettings {
                    private _requestWrapper;
                    private _isUriValid;
                    private _lastValidatedJsonSourceJSON;
                    private _authNameValidatorInstance;
                    private _collectionItemNamePlaceholder;
                    private _lastValidateDeferred;
                    private _sourceUriValidatorsReady;
                    private _basicAuthValidatorsReady;
                    private _validationRequested;
                    private _validateUriSource;
                    private _isCollectionValid;
                    private _isHeadersValid;
                    private _isQueryParametersValid;
                    private _isBasicHttpAuthValid;
                    private _noEmptyProperties;
                    private _lastValidationMessage;
                    private _getSerializedUriSource;
                    _sourceUriValidationCallback: (params: any) => boolean;
                    private _getSourceUriInfo;
                    private _getBasicHttpAuthInfo;
                    private _getQueryParametersInfo;
                    private _getHttpHeadersInfo;
                    constructor(_requestWrapper: QueryBuilder.Utils.RequestWrapper);
                    applySettings(jsonDataSource: Data.JsonDataSource): void;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    reset(): void;
                    setValue(dataSource: Data.JsonDataSource): void;
                    dispose(): void;
                    onChange(_onChange: () => void): any;
                    isEmpty(): boolean;
                    isValid: ko.PureComputed<boolean>;
                    _validate(): void;
                    _validatorsReady: ko.PureComputed<boolean>;
                    sourceUri: ko.Observable<string>;
                    basicHttpAuth: {
                        password: ko.Observable<string>;
                        userName: ko.Observable<string>;
                    };
                    queryParameters: ko.ObservableArray<Data.JsonQueryParameter>;
                    headers: ko.ObservableArray<Data.JsonHeaderParameter>;
                }
            }
            interface IJsonDataSourceJsonSourcePageSettings extends IJsonDataSourceJsonSourceValidatable {
                isValid(): boolean;
                reset(): void;
                setValue(dataSource: Data.JsonDataSource): void;
                isEmpty(): boolean;
                applySettings(dataSource: Data.JsonDataSource): void;
                cssClass?: string | any;
                grid?: Widgets.ObjectProperties;
            }
            interface IJsonDataSourceJsonSourceValidatable {
                validationGroup?: {
                    onInitialized: (args: any) => void;
                    validate: () => any;
                    onDisposing: (args: any) => void;
                };
                validationSummary?: {
                    onInitialized: (args: any) => void;
                    onDisposing: (args: any) => void;
                };
                _validatorsReady?: ko.Observable<boolean> | ko.Computed<boolean>;
                _validate?: () => void;
            }
            interface IJsonDataSourceType {
                value: IJsonDataSourceJsonSourcePageSettings;
                displayValue: string;
                localizationId: string;
            }
            module Internal {
                interface IDataSourceWizardCallbacks {
                    selectStatement?: (connection: Data.SqlDataConnection, queryJSON: string) => JQueryPromise<QueryBuilder.Utils.ISelectStatementResponse>;
                    finishCallback?: (wizardModel: any) => JQueryPromise<any>;
                    customQueriesPreset?: (dataSource: Data.SqlDataSource) => JQueryPromise<Data.Utils.ISqlQueryViewModel[]>;
                    customizeQBInitData?: (data: any) => any;
                    validateJsonUri?: (data: any) => any;
                }
                interface IMultiQueryDataSourceWizardCallbacks extends IDataSourceWizardCallbacks {
                    sqlDataSourceResultSchema?: (dataSource: Data.SqlDataSource) => JQueryPromise<{
                        resultSchemaJSON: string;
                        connectionParameters?: string;
                    }>;
                }
                interface IParametersViewModelConverter {
                    createParameterViewModel(parameter: Data.DataSourceParameter): any;
                    getParameterFromViewModel(parameterViewModel: any): Data.DataSourceParameter;
                }
                function subscribeArray<T>(array: ko.ObservableArray<T>, subscribeItem: (value: T, onChange: () => void) => void, onChange: () => void): ko.Subscription;
                function subscribeProperties(properties: Array<ko.Observable<any> | ko.Computed<any>>, onChange: () => void): any[];
                function subscribeObject<T>(object: ko.Observable<T> | ko.Computed<T>, subscribeProperties: (value: T, onChange: () => void) => void, onChange: () => void): ko.Subscription;
                function _createBeforeInitializePageEventArgs<TWizard extends BaseWizard | WizardPageProcessor>(page: _WrappedWizardPage, self: TWizard): IBeforeWizardPageInitializeEventArgs<TWizard>;
                function _createPageEventArgs<TWizard extends BaseWizard | WizardPageProcessor>(page: _WrappedWizardPage, self: TWizard): IWizardPageEventArgs<TWizard>;
                class ParametersTreeListItem extends Utils.Disposable implements DevExpress.Analytics.Utils.IDataMemberInfo {
                    parent: ParametersTreeListRootItem;
                    private _name;
                    constructor(parameter: {
                        name: ko.Observable<string> | ko.Computed<string>;
                    }, parent: ParametersTreeListRootItem);
                    dataSourceParameter: ko.Observable<{
                        name: ko.Observable<string> | ko.Computed<string>;
                    }> | ko.Computed<{
                        name: ko.Observable<string> | ko.Computed<string>;
                    }>;
                    editor: any;
                    isList: boolean;
                    contenttemplate: string;
                    actionsTemplate: string;
                    readonly name: string;
                    readonly displayName: string;
                    query(): Data.Utils.ISqlQueryViewModel;
                }
                class ParametersTreeListRootItem implements DevExpress.Analytics.Utils.IDataMemberInfo {
                    private _query;
                    constructor(query: Data.Utils.ISqlQueryViewModel);
                    name: string;
                    displayName: string;
                    isList: boolean;
                    specifics: string;
                    parameters: ko.ObservableArray<ParametersTreeListItem>;
                    removeChild(parameter: any): void;
                    query(): Data.Utils.ISqlQueryViewModel;
                }
                class ParametersTreeListController extends Analytics.Widgets.Internal.TreeListController {
                    private _createNewParameter;
                    private _rootItems;
                    constructor(rootItems: ParametersTreeListRootItem[], createNewParameter: (queryName: string, parameters: {
                        name: string;
                    }[]) => any);
                    hasItems(item: Analytics.Utils.IDataMemberInfo): boolean;
                    getActions(treeListItem: Analytics.Widgets.Internal.TreeListItemViewModel & {
                        data: ParametersTreeListRootItem | ParametersTreeListItem;
                    }): Utils.IAction[];
                    canSelect(value: Analytics.Widgets.Internal.TreeListItemViewModel): boolean;
                }
                interface IAddQueriesTreeListCallbacks {
                    deleteAction: (name: string) => any;
                    showQbCallBack: (name?: string, isCustomQuery?: boolean) => any;
                    disableCustomSql: boolean;
                }
                class DBSchemaItemsProvider implements Analytics.Utils.IItemsProvider {
                    private _callBack;
                    private _tables;
                    private _views;
                    private _procedures;
                    private _queries;
                    private _customQueries;
                    private _rootItems;
                    constructor(dbSchemaProvider: Data.DBSchemaProvider, customQueries: ko.ObservableArray<Data.TableQuery>, showQbCallBack: any, disableCustomSql: any, afterCheckToggled: (node: TreeNodeBase) => void);
                    private _checkedRootNodesCount;
                    getItems: (path: DevExpress.Analytics.Utils.IPathRequest) => JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
                    hasCheckedItems: ko.PureComputed<boolean>;
                    nextButtonDisabled: ko.PureComputed<boolean>;
                    hasParametersToEdit: ko.PureComputed<boolean>;
                    tables: () => TreeNode;
                    views: () => TreeNode;
                    procedures: () => ParameterTreeNode;
                    queries: () => QueriesTreeNode;
                    customQueries: () => ko.ObservableArray<Data.Utils.ISqlQueryViewModel>;
                }
                class DBSchemaTreeListController extends Analytics.Widgets.Internal.TreeListController {
                    private _customizeDBSchemaTreeListActions;
                    constructor(_customizeDBSchemaTreeListActions: (item: Analytics.Utils.IDataMemberInfo, actions: Analytics.Utils.IAction[]) => void);
                    getActions(value: Analytics.Widgets.Internal.TreeListItemViewModel): Utils.IAction[];
                    canSelect(value: Analytics.Widgets.Internal.TreeListItemViewModel): boolean;
                }
                class QueryBuilderPopup {
                    customizeQBInitializationData: (data: any) => any;
                    private _applyQuery;
                    private _query;
                    private _dbSchemaProvider;
                    private _dataSource;
                    private _rtl;
                    constructor(applyNewQuery: IRetrieveQuerySqlCallback, rtl?: boolean, customizeQBInitializationData?: (data: any) => any);
                    designer: ko.Observable<{
                        model: ko.Observable<QueryBuilder.Elements.QueryViewModel>;
                        updateSurface: () => void;
                        showPreview: () => void;
                        dataPreview: any;
                    }>;
                    qbOptions: ko.Observable<any>;
                    okButtonDisabled: ko.PureComputed<boolean>;
                    isVisible: ko.Observable<boolean>;
                    showLoadIndicator: ko.Observable<boolean>;
                    static customizeQueryBuilderActions: (actions: Utils.IAction[]) => void;
                    show(query: Data.TableQuery, dataSource: Data.SqlDataSource): void;
                    cancelHandler(): void;
                    previewHandler(): void;
                    okHandler(): void;
                    onHiddenHandler(): void;
                    popupViewModel(element: HTMLElement): {
                        visible: ko.Observable<boolean>;
                        title: any;
                        showTitle: boolean;
                        shading: boolean;
                        fullScreen: boolean;
                        width: string;
                        height: string;
                        container: JQuery;
                        position: {
                            of: JQuery;
                        };
                        onHidden: () => void;
                    };
                    getDisplayText(key: any): any;
                    localizationIdMap: {
                        [key: string]: Analytics.Internal.ILocalizationInfo;
                    };
                }
                class SelectQuerySqlTextProvider {
                    private _selectStatementCallback;
                    private _connection;
                    constructor(_selectStatementCallback: (connection: Data.SqlDataConnection, queryJSON: string) => JQueryPromise<QueryBuilder.Utils.ISelectStatementResponse>, _connection: () => Data.SqlDataConnection);
                    getQuerySqlText(newQuery: Data.TableQuery): JQueryPromise<QueryBuilder.Utils.ISelectStatementResponse>;
                }
                interface ISqlQueryControl {
                    isNextDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    isFinishDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    setQuery: (query: Data.Utils.ISqlQueryViewModel, isInProcess?: ko.Observable<boolean>) => void;
                    getQuery: () => Data.Utils.ISqlQueryViewModel;
                    runQueryBuilderDisabled: boolean;
                }
                class SelectStatementQueryControl implements ISqlQueryControl {
                    private _tableQueryString;
                    private _query;
                    private _needToCustomizeParameters;
                    private _sqlTextProvider;
                    constructor(sqlTextProvider: SelectQuerySqlTextProvider, disableCustomSql: any);
                    template: string;
                    aceOptions: {
                        showLineNumbers: boolean;
                        showPrintMargin: boolean;
                        enableBasicAutocompletion: boolean;
                        enableLiveAutocompletion: boolean;
                        readOnly: boolean;
                        highlightSelectedWord: boolean;
                        showGutter: boolean;
                        highlightActiveLine: boolean;
                    };
                    additionalOptions: {
                        onChange: (session: any) => void;
                        onValueChange: (editor: any) => void;
                        changeTimeout: number;
                        overrideEditorFocus: boolean;
                        setUseWrapMode: boolean;
                    };
                    aceAvailable: boolean;
                    languageHelper: {
                        getLanguageMode: () => string;
                        createCompleters: () => any[];
                    };
                    caption: () => any;
                    sqlString: ko.PureComputed<string>;
                    setQuery(query: Data.Utils.ISqlQueryViewModel, isInProcess?: ko.Observable<boolean>): JQueryPromise<QueryBuilder.Utils.ISelectStatementResponse>;
                    getQuery(): Data.Utils.ISqlQueryViewModel;
                    isNextDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    isFinishDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    readonly runQueryBuilderDisabled: boolean;
                    disableCustomSql: () => boolean;
                }
                class StoredProceduresQueryControl extends Analytics.Utils.Disposable implements ISqlQueryControl {
                    private _query;
                    private _needToProcessParameters;
                    private static _availableConvertToParameter;
                    private _selectedProcedure;
                    constructor();
                    template: string;
                    storedProcedures: ko.ObservableArray<Data.DBStoredProcedure>;
                    selectedProcedure: ko.ObservableArray<Data.DBStoredProcedure>;
                    caption: () => any;
                    generateStoredProcedureDisplayName: (procedure: any) => string;
                    scrollActiveItem(e: any): void;
                    static generateStoredProcedureDisplayName(procedure: Data.DBStoredProcedure): string;
                    setQuery(query: Data.StoredProcQuery): void;
                    getQuery(): Data.StoredProcQuery;
                    isNextDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    isFinishDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    readonly runQueryBuilderDisabled: boolean;
                }
                var defaultObjectDataSourceItemSpecifics: string;
                class TreeNodeBase implements DevExpress.Analytics.Utils.IDataMemberInfo {
                    name: string;
                    displayName: string;
                    specifics: string;
                    private _afterCheckToggled;
                    constructor(name: string, displayName: string, specifics: string, isChecked?: boolean, afterCheckToggled?: (node: TreeNodeBase) => void);
                    checked: ko.PureComputed<boolean>;
                    unChecked(): boolean;
                    toggleChecked(): void;
                    setChecked(value: boolean): void;
                    isList: boolean;
                    _checked: ko.Observable<boolean> | ko.Computed<boolean>;
                }
                class TreeLeafNode extends TreeNodeBase {
                    name: string;
                    displayName: string;
                    specifics: string;
                    constructor(name: string, displayName: string, specifics: string, isChecked?: boolean, nodeArguments?: any, afterCheckToggled?: (node: TreeNodeBase) => void);
                    arguments: Data.DBStoredProcedureArgument[];
                    hasQuery: boolean;
                }
                class TreeNode extends TreeNodeBase {
                    constructor(name: string, displayName: string, specifics: string, isChecked: boolean, afterCheckToggled?: (node: TreeNodeBase) => void);
                    initialized(): boolean;
                    setChecked(value: boolean): void;
                    initializeChildren(children: TreeNodeBase[]): void;
                    countChecked: ko.PureComputed<number>;
                    isList: boolean;
                    children: ko.ObservableArray<TreeNodeBase>;
                }
                class ParameterTreeNode extends TreeNode {
                    constructor(name: string, displayName: string, specifics: string, isChecked: boolean, afterCheckToggled?: (node: TreeNodeBase) => void);
                    countChecked: ko.PureComputed<number>;
                    hasParamsToEdit: ko.Observable<boolean>;
                }
                class QueriesTreeNode extends ParameterTreeNode {
                    constructor(name: string, displayName: string, specifics: string, isChecked: boolean, callbacks?: ko.Observable<IAddQueriesTreeListCallbacks>, afterCheckToggled?: (node: TreeNodeBase) => void);
                    addAction: {
                        clickAction: (item: any) => any;
                        imageClassName: string;
                        imageTemplateName: string;
                        templateName: string;
                        text: any;
                    };
                    getActions(context: any): Utils.IAction[];
                    popoverListItems(): any;
                    showPopover(): void;
                    itemClickAction: (e: {
                        itemData: {
                            name: string;
                            addAction: any;
                        };
                    }) => void;
                    addQuery: any;
                    addCustomQuery: any;
                    popoverVisible: ko.Observable<boolean> | ko.Computed<boolean>;
                    disableCustomSql: () => boolean;
                }
                class TreeQueryNode extends TreeLeafNode {
                    private _name;
                    constructor(name: string, displayName: string, specifics: string, isChecked: boolean, parameters: ko.Observable<Data.DataSourceParameter[]>, callbacks: ko.Observable<IAddQueriesTreeListCallbacks>, afterCheckToggled?: (node: TreeLeafNode) => void);
                    setObservableName(name: ko.Observable<string> | ko.Computed<string>): void;
                    editAction: {
                        clickAction: (item: any) => any;
                        imageClassName: string;
                        imageTemplateName: string;
                        text: any;
                    };
                    removeAction: {
                        clickAction: (item: any) => void;
                        imageClassName: string;
                        imageTemplateName: string;
                        text: any;
                    };
                    getActions(context: any): Utils.IAction[];
                    editQuery: any;
                    removeQuery: any;
                    parameters: ko.Observable<Data.DataSourceParameter[]>;
                }
                class FieldTreeNode extends Analytics.Wizard.Internal.TreeNodeBase {
                    constructor(name: string, displayName: string, specifics: string, isChecked: boolean, path: string, afterCheckToggled?: (node: Analytics.Wizard.Internal.TreeNodeBase) => void);
                    path: string;
                    visible: ko.Observable<boolean>;
                    isComplex: boolean;
                }
                class DataMemberTreeNode extends Analytics.Wizard.Internal.TreeNode {
                    constructor(name: string, displayName: string, specifics: string, isChecked: boolean, path: string, afterCheckToggled?: (node: DataMemberTreeNode) => void);
                    setChecked(value: boolean): void;
                    path: string;
                    visible: ko.Observable<boolean>;
                    children: ko.ObservableArray<DataMemberTreeNode | FieldTreeNode>;
                    isComplex: boolean;
                }
                class TreeNodeItemsProvider extends Utils.Disposable implements Analytics.Utils.IItemsProvider {
                    private _fullTreeLoaded;
                    protected _rootItems: ko.ObservableArray<Internal.DataMemberTreeNode>;
                    private _checkedRootNodesCount;
                    private _createTree;
                    private _createTreePart;
                    private _setChecked;
                    selectAllItems(onlyRoot?: boolean): JQueryPromise<{}>;
                    selectItemsByPath(path: string): JQueryPromise<{}>;
                    selectItemByPath(path: string): JQueryPromise<{}>;
                    protected _getParentNode(pathRequest: DevExpress.Analytics.Utils.IPathRequest): DataMemberTreeNode;
                    protected _getDefaultTreeNodeCheckState(item: Analytics.Utils.IDataMemberInfo): boolean;
                    constructor(fieldListProvider: Analytics.Internal.FieldListProvider, rootItems: ko.ObservableArray<Analytics.Utils.IDataMemberInfo>, generateTreeNode: (item: Analytics.Utils.IDataMemberInfo, isChecked: boolean, path: string) => Internal.DataMemberTreeNode, generateTreeLeafNode: (item: Analytics.Utils.IDataMemberInfo, isChecked: boolean, path: string) => Internal.FieldTreeNode);
                    hasCheckedItems: ko.Computed<boolean>;
                    getItems: (path: DevExpress.Analytics.Utils.IPathRequest, collectChilds?: boolean) => JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
                    getRootItems: () => DataMemberTreeNode[];
                }
                class JsonTreeNodeItemsProvider extends TreeNodeItemsProvider implements Analytics.Utils.IItemsProvider {
                    constructor(fieldListProvider: Analytics.Internal.FieldListProvider, rootItems: ko.ObservableArray<Analytics.Utils.IDataMemberInfo>, generateTreeNode: (item: Analytics.Utils.IDataMemberInfo, isChecked: boolean, path: string) => Internal.DataMemberTreeNode, generateTreeLeafNode: (item: Analytics.Utils.IDataMemberInfo, isChecked: boolean, path: string) => Internal.FieldTreeNode);
                    protected _getDefaultTreeNodeCheckState(item: Analytics.Utils.IDataMemberInfo): boolean;
                    getNodeByPath(pathRequest: DevExpress.Analytics.Utils.IPathRequest): DataMemberTreeNode;
                }
            }
            module Legacy {
                interface IPageInfo<TWizardModel> {
                    page: WizardPage<TWizardModel>;
                    token: any;
                    isFirst: boolean;
                    isLast: boolean;
                }
                interface IPageDispatcher<TWizardModel> {
                    getFirstPage(model: TWizardModel): JQueryPromise<IPageInfo<TWizardModel>>;
                    getNextPage(currentToken: any, model: TWizardModel): JQueryPromise<IPageInfo<TWizardModel>>;
                    getPreviousPage(currentToken: any, model: TWizardModel): JQueryPromise<IPageInfo<TWizardModel>>;
                    getPageByIndex(index: number, model: TWizardModel): JQueryPromise<IPageInfo<TWizardModel>>;
                }
                class LegacyPageDispathcer<TWizardModel> implements IPageDispatcher<TWizardModel> {
                    private _steps;
                    constructor(steps: WizardPage<TWizardModel>[]);
                    getFirstPage(model: TWizardModel): JQueryPromise<any>;
                    getNextPage(currentToken: any, model: TWizardModel): JQueryPromise<any>;
                    getPreviousPage(currentToken: any, model: TWizardModel): JQueryPromise<any>;
                    getPageByIndex(index: number, model: TWizardModel): JQueryPromise<any>;
                    private _isPageFirst;
                    private _isPageLast;
                    private _goToFirstVisiblePage;
                }
            }
            module Internal {
                class WizardAction {
                    constructor(handler: () => void, text: string);
                    isVisible: ko.Observable<boolean> | ko.Computed<boolean>;
                    isDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    handler: () => void;
                    text: string;
                }
            }
        }
    }
    module QueryBuilder {
        module Widgets {
            var expressionFunctions: Analytics.Widgets.Internal.IExpressionEditorFunction[];
            module Internal {
                class UndoEditor extends Analytics.Widgets.Editor {
                    constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                    generateValue(undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>): ko.Computed<any> | ko.Observable<any>;
                    undoValue: ko.Observable | ko.Computed;
                }
            }
            var editorTemplates: {
                bool: {
                    header: string;
                    custom: string;
                };
                combobox: {
                    header: string;
                    custom: string;
                };
                comboboxUndo: {
                    header: string;
                    custom: string;
                    editorType: typeof Internal.UndoEditor;
                };
                text: {
                    header: string;
                    custom: string;
                };
                filterEditor: {
                    header: string;
                    custom: string;
                };
                filterGroupEditor: {
                    header: string;
                    custom: string;
                };
                numeric: {
                    header: string;
                    custom: string;
                };
            };
            module Internal {
                function createDefaultSQLAceOptions(readOnly?: boolean): {
                    showLineNumbers: boolean;
                    showPrintMargin: boolean;
                    enableBasicAutocompletion: boolean;
                    enableLiveAutocompletion: boolean;
                    readOnly: boolean;
                    highlightSelectedWord: boolean;
                    showGutter: boolean;
                    highlightActiveLine: boolean;
                };
                function createDefaultSQLAdditionalOptions(value: any): {
                    onChange: (session: any) => void;
                    onValueChange: (editor: any) => void;
                    changeTimeout: number;
                    overrideEditorFocus: boolean;
                    setUseWrapMode: boolean;
                };
                function createDefaultSQLLanguageHelper(): {
                    getLanguageMode: () => string;
                    createCompleters: () => any[];
                };
                class GroupFilterEditorSerializer extends Analytics.Widgets.Internal.FilterEditorSerializer {
                    private _columns;
                    private _columnDisplayName;
                    private _findAggregatedColumn;
                    private _aggregatePropertyName;
                    constructor(_columns: () => Elements.ColumnExpression[]);
                    serializeOperandProperty(operand: Analytics.Criteria.OperandProperty): string;
                    deserialize(stringCriteria: string): Analytics.Criteria.CriteriaOperator;
                }
                class OperandParameterQBSurface extends DevExpress.Analytics.Widgets.Filtering.OperandParameterSurface {
                    static defaultDisplay: () => any;
                    private readonly _parameterType;
                    constructor(operator: DevExpress.Analytics.Criteria.OperandParameter, parent: any, fieldListProvider?: any, path?: any);
                    _createParameter(name: string, dataType: string): void;
                    createParameter: () => void;
                    fieldListProvider: ko.Observable<QueryBuilderObjectsProvider>;
                    _parameterName: ko.Observable<string>;
                    isEditable: ko.Observable<boolean> | ko.Computed<boolean>;
                    fieldsOptions: any;
                    helper: QBFilterEditorHelper;
                    canCreateParameters: boolean;
                    isDefaultTextDisplayed(): boolean;
                    defaultDisplay: () => any;
                }
                class OperandPropertyQBSurface extends DevExpress.Analytics.Widgets.Filtering.OperandPropertySurface {
                    _updateSpecifics(): void;
                    constructor(operator: Analytics.Criteria.OperandProperty, parent: any, fieldListProvider?: QueryBuilderObjectsProvider, path?: any);
                    fieldListProvider: ko.Observable<QueryBuilderObjectsProvider>;
                    static updateSpecifics(propertySurface: {
                        fieldListProvider: ko.Observable<{
                            getColumnInfo: (path: string) => Analytics.Utils.IDataMemberInfo;
                        }>;
                        propertyName: ko.Observable<string>;
                        specifics: ko.Observable<string>;
                        dataType: ko.Observable<string>;
                        fieldsOptions?: ko.Observable<{
                            selected: ko.Observable<any>;
                        }>;
                    }): void;
                }
                function isAggregatedExpression(object: {
                    aggregate: ko.Observable<string> | ko.Computed<string>;
                }): boolean;
                interface IQueryBuilderObjectProviderFilter {
                    filterTables(tables: Elements.TableViewModel[]): Elements.TableViewModel[];
                    filterColumns(columns: Elements.ColumnViewModel[]): Elements.ColumnViewModel[];
                    getColumnName(column: Elements.ColumnViewModel): string;
                    getSpecifics(column: Elements.ColumnViewModel): string;
                    getDataType(column: Elements.ColumnViewModel): string;
                }
                class QueryBuilderObjectsProvider implements DevExpress.Analytics.Utils.IItemsProvider {
                    constructor(query: ko.Observable<Elements.QueryViewModel>, objectFilter: IQueryBuilderObjectProviderFilter);
                    hasParameter: (name: string) => boolean;
                    createParameter: (name: any, dataType: any) => void;
                    getItems: (IPathRequest: any) => JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
                    getColumnInfo: (propertyName: string) => DevExpress.Analytics.Utils.IDataMemberInfo;
                    private static _createTableInfo;
                    private static _createColumnInfo;
                    static whereClauseObjectsFilter: IQueryBuilderObjectProviderFilter;
                    static groupByObjectsFilter: IQueryBuilderObjectProviderFilter;
                }
                class QBFilterEditorHelper extends Analytics.Widgets.FilterEditorHelper {
                    constructor(parametersMode: string);
                    newParameters: ko.ObservableArray<Analytics.Data.DataSourceParameter>;
                }
                var QBFilterEditorHelperDefault: typeof QBFilterEditorHelper;
                class QBFilterStringOptions extends Analytics.Widgets.FilterStringOptions {
                    constructor(filterString: ko.Observable<string> | ko.Computed<string>, dataMember?: ko.Observable | ko.Computed, disabled?: ko.Observable<boolean> | ko.Computed<boolean>, title?: {
                        text: string;
                        localizationId?: string;
                    });
                    initializeFilterStringHelper(parameters: ko.ObservableArray<Elements.ParameterViewModel> | ko.Computed<Elements.ParameterViewModel[]>, parametersMode: string, serializer?: Analytics.Widgets.Internal.FilterEditorSerializer): void;
                    helper: QBFilterEditorHelper;
                }
                class KeyColumnSurface {
                    private _isMaster;
                    constructor(column: ko.Observable<string> | ko.Computed<string>, queryName: string, _isMaster?: boolean);
                    getTitle: () => any;
                    isSelected: ko.Observable<boolean> | ko.Computed<boolean>;
                    setColumn: (resultColumn: {
                        name: ko.Observable<string> | ko.Computed<string>;
                        propertyType: ko.Observable<string> | ko.Computed<string>;
                    }) => void;
                    queryName: string;
                    column: ko.Observable<string> | ko.Computed<string>;
                    selectColumnText: () => any;
                }
                class MasterDetailEditor {
                    private _createMainPopupButtons;
                    constructor(relations: ko.ObservableArray<Analytics.Data.MasterDetailRelation>, resultSet: Analytics.Data.ResultSet, saveCallBack: () => JQueryPromise<{}>);
                    isValid: ko.Observable<boolean>;
                    save: () => void;
                    popupVisible: ko.Observable<boolean>;
                    loadPanelVisible: ko.Observable<boolean>;
                    buttonItems: any[];
                    popupService: DevExpress.Analytics.Internal.PopupService;
                    masterQueries: ko.ObservableArray<MasterQuerySurface>;
                    createRelation: (target: MasterQuerySurface) => any;
                    setColumn: (target: KeyColumnSurface) => any;
                    title(): any;
                }
                class MasterDetailEditorPopupManager {
                    private _popupService;
                    private _action;
                    private _popupItems;
                    private _updateActions;
                    constructor(target: any, popupService: DevExpress.Analytics.Internal.PopupService, action: string, popupItems: {
                        name: any;
                    }[]);
                    target: any;
                    showPopup: (_: any, element: any) => void;
                }
                class MasterDetailRelationSurface {
                    constructor(relation: Analytics.Data.MasterDetailRelation, parent: MasterQuerySurface);
                    relationName: ko.Observable<string> | ko.Computed<string>;
                    keyColumns: ko.Computed<{
                        master: KeyColumnSurface;
                        detail: KeyColumnSurface;
                    }[]>;
                    isEditable: ko.Observable<boolean> | ko.Computed<boolean>;
                    create: () => void;
                    remove: (data: {
                        master: KeyColumnSurface;
                        detail: KeyColumnSurface;
                    }) => void;
                }
                class MasterQuerySurface {
                    constructor(masterQueryName: string, relations: ko.ObservableArray<Analytics.Data.MasterDetailRelation>);
                    queryName: string;
                    relations: ko.ObservableArray<MasterDetailRelationSurface>;
                    create: (detailQueryItem: {
                        name: string;
                    }) => void;
                    add: (relation: Analytics.Data.MasterDetailRelation) => void;
                    remove: (relationSurface: MasterDetailRelationSurface) => void;
                }
            }
        }
        module Metadata {
            var name: Analytics.Utils.ISerializationInfo;
            var alias: Analytics.Utils.ISerializationInfo;
            var text: Analytics.Utils.ISerializationInfo;
            var selected: Analytics.Utils.ISerializationInfo;
            var size: Analytics.Utils.ISerializationInfo;
            var location: Analytics.Utils.ISerializationInfo;
            var sizeLocation: Analytics.Utils.ISerializationInfoArray;
            var unknownSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
        }
        module Utils {
            var ActionId: {
                Save: string;
                DataPreview: string;
                SelectStatementPreview: string;
            };
            var HandlerUri: string;
        }
        module Internal {
            function updateQueryBuilderSurfaceContentSize($root: JQuery, surfaceSize: ko.Observable<number> | ko.Computed<number>, surface: ko.Observable<Elements.QuerySurface>, updateLayoutCallbacks?: Array<() => void>): () => void;
            function createIsLoadingFlag(model: ko.Observable<Elements.QueryViewModel>, dbSchemaProvider: ko.Observable<Analytics.Data.IDBSchemaProvider>): ko.PureComputed<boolean>;
            var isJoinsResolvingDisabled: boolean;
            function createQueryBuilder(element: Element, data: {
                querySource: ko.Observable<{}>;
                dbSchemaProvider: ko.Observable<Analytics.Data.DBSchemaProvider>;
                parametersMode?: string;
                parametersItemsProvider?: Analytics.Utils.IItemsProvider;
                requestWrapper?: Utils.RequestWrapper;
            }, callbacks?: Analytics.Internal.ICommonCustomizationHandler, localization?: any, rtl?: boolean): JQueryDeferred<any>;
            interface ITabModel {
                editableObject: any;
                properties: Analytics.Widgets.ObjectProperties;
            }
            interface IItemPropertiesTabModel extends ITabModel {
                fieldListModel: {
                    treeListOptions: ko.Observable<Analytics.Widgets.Internal.ITreeListOptions> | ko.Computed<Analytics.Widgets.Internal.ITreeListOptions>;
                };
                searchName: ko.Observable<string> | ko.Computed<string>;
                tablesTop: ko.Observable<number> | ko.Computed<number>;
                searchPlaceholder: () => string;
            }
            class AccordionTabInfo extends Analytics.Utils.TabInfo {
                static _getSelectedItemPropertyName(model: any): string;
                static _createWrappedObject(query: any, commonModel: any, undoEngine: any, showParameters: boolean): {
                    selectedItem: any;
                    query: {
                        editableObject: any;
                        properties: Analytics.Widgets.ObjectProperties;
                    };
                    fields: any;
                    isPropertyVisible: (propertyName: string) => boolean;
                };
                static _createGroups(editableObject: ko.Observable<any>, showParameters: boolean): Analytics.Internal.GroupObject;
                static _createQBPropertyGrid(query: ko.Observable<Elements.QueryViewModel> | ko.Computed<Elements.QueryViewModel>, commonModel: IItemPropertiesTabModel, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, showParameters: boolean): Analytics.Internal.ControlProperties;
                private _getGroupByName;
                constructor(query: ko.Observable<Elements.QueryViewModel> | ko.Computed<Elements.QueryViewModel>, itemPropertiesTabInfoModel: IItemPropertiesTabModel, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, focused: ko.Observable | ko.Computed, showParameters: boolean);
                model: DevExpress.Analytics.Internal.ControlProperties;
            }
            class ColumnExpressionCollectionHelper {
                static find(collection: ko.ObservableArray<Elements.ColumnExpression>, tableName: string, columnName: string): Elements.ColumnExpression;
                static findByName(collection: ko.ObservableArray<Elements.ColumnExpression>, actualName: string): Elements.ColumnExpression;
                static removeDependend(collection: ko.ObservableArray<Elements.ColumnExpression>, tableName: string): void;
                static setUniqueAlias(collection: any, alias: any): any;
                static addNew(query: Elements.QueryViewModel, collection: ko.ObservableArray<Elements.ColumnExpression>, table: string, column: string): Elements.ColumnExpression;
                static remove(collection: ko.ObservableArray<Elements.ColumnExpression>, tableName: string, columnName: string): void;
            }
        }
        module Utils {
            var controlsFactory: Analytics.Utils.ControlsFactory;
        }
        module Internal {
            function registerControls(): void;
            class QueryBuilderTreeListController extends Analytics.Widgets.Internal.TreeListController {
                constructor(undoEngine: ko.Observable<Analytics.Utils.UndoEngine>, query: ko.Observable<Elements.QueryViewModel>, searchName: ko.Observable<string> | ko.Computed<string>);
                dblClickHandler: (item: Analytics.Widgets.Internal.TreeListItemViewModel) => void;
                searchName: ko.Observable<string> | ko.Computed<string>;
            }
        }
        module Utils {
            interface ISelectStatementResponse {
                sqlSelectStatement: string;
                errorMessage: string;
            }
            interface IUriJsonSourceValidationResult {
                isUriValid: boolean;
                faultMessage?: string;
            }
            class RequestWrapper {
                sendRequest<T = any>(action: string, arg: string): JQueryPromise<T>;
                _sendRequest<T = any>(settings: Analytics.Internal.IAjaxSettings): JQueryPromise<T>;
                getDbSchema(connection: Analytics.Data.SqlDataConnection, tables?: Analytics.Data.DBTable[]): JQueryPromise<{
                    dbSchemaJSON: string;
                }>;
                getDbStoredProcedures(connection: Analytics.Data.SqlDataConnection): JQueryPromise<{
                    dbSchemaJSON: string;
                }>;
                getSelectStatement(connection: Analytics.Data.SqlDataConnection, queryJSON: string): JQueryPromise<ISelectStatementResponse>;
                getDataPreview(connection: Analytics.Data.SqlDataConnection, queryJSON: string): JQueryPromise<{
                    dataPreviewJSON: string;
                }>;
                getConnectionJSON(connection: Analytics.Data.SqlDataConnection): string;
                rebuildResultSchema(dataSource: Analytics.Data.SqlDataSource, queryName?: string, relationsEditing?: boolean): JQueryPromise<{
                    resultSchemaJSON: string;
                    connectionParameters?: string;
                }>;
                validateJsonUri(jsonDataSource: Analytics.Data.JsonDataSource): JQueryPromise<IUriJsonSourceValidationResult>;
                saveJsonSource(connectionName: string, jsonDataSource: Analytics.Data.JsonDataSource): JQueryPromise<string>;
                getJsonSchema(jsonDataSource: Analytics.Data.JsonDataSource): JQueryPromise<{
                    jsonSchemaJSON: string;
                }>;
            }
        }
        module Internal {
            function wrapGetSelectStatement(callback?: (connection: Analytics.Data.SqlDataConnection, queryJSON: string) => JQueryPromise<Utils.ISelectStatementResponse>): (connection: Analytics.Data.SqlDataConnection, queryJSON: string) => JQueryPromise<Utils.ISelectStatementResponse>;
            function wrapRebuildResultSchema(callback?: (dataSource: Analytics.Data.SqlDataSource, queryName?: string, relationsEditing?: boolean) => JQueryPromise<{
                resultSchemaJSON: string;
                connectionParameters?: string;
            }>): (dataSource: Analytics.Data.SqlDataSource, queryName?: string, relationsEditing?: boolean) => JQueryPromise<{
                resultSchemaJSON: string;
                connectionParameters?: string;
            }>;
        }
        module Elements {
            class QueryElementBaseViewModel extends Analytics.Elements.ElementViewModel {
                getControlFactory(): Analytics.Utils.ControlsFactory;
                constructor(control: any, parent: Analytics.Elements.ElementViewModel, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                size: Analytics.Elements.Size;
                location: Analytics.Elements.Point;
            }
            module Metadata {
                var allColumnsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class AllColumnsViewModel extends QueryElementBaseViewModel {
                constructor(parent: TableViewModel, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                selected: ko.Observable<boolean> | ko.Computed<boolean>;
                name: ko.Computed<string>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class AllColumnsSurface extends Analytics.Elements.SurfaceElementBase<AllColumnsViewModel> {
                constructor(control: AllColumnsViewModel, context: Analytics.Elements.ISurfaceContext);
                template: string;
                toggleSelected: () => void;
                selectedWrapper: ko.PureComputed<boolean>;
                isOverAsterisk: ko.PureComputed<boolean>;
                cssClasses: () => {
                    'dxd-state-active': ko.Computed<boolean> | ko.Observable<boolean>;
                    'dxd-state-hovered': boolean;
                };
            }
            module Metadata {
                var AggregationType: {
                    None: string;
                    Count: string;
                    Max: string;
                    Min: string;
                    Avg: string;
                    Sum: string;
                    CountDistinct: string;
                    AvgDistinct: string;
                    SumDistinct: string;
                };
                var columnSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class ColumnViewModel extends QueryElementBaseViewModel {
                private _isAliasAutoGenerated;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, dbColumn: Analytics.Data.DBColumn, parent: TableViewModel, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                isPropertyDisabled(name: string): boolean;
                name: ko.Observable<string> | ko.Computed<string>;
                alias: ko.Observable<string> | ko.Computed<string>;
                selected: ko.Observable<boolean> | ko.Computed<boolean>;
                actualName: ko.Computed<string>;
                displayType: ko.Computed<string>;
                dataType: ko.Computed<string>;
                rightConnectionPoint: Analytics.Diagram.IConnectingPoint;
                leftConnectionPoint: Analytics.Diagram.IConnectingPoint;
                sortingType: ko.Computed<string>;
                sortOrder: ko.Computed<number>;
                groupBy: ko.Computed<boolean>;
                aggregate: ko.Observable<string> | ko.Computed<string>;
                readonly specifics: "String" | "Date" | "Bool" | "Integer" | "Float";
            }
            module Metadata {
                var ColumnType: {
                    RecordsCount: string;
                    Column: string;
                    Expression: string;
                    AllColumns: string;
                };
                var columnExpressionSerializationsInfo: ({
                    propertyName: string;
                    modelName: string;
                    defaultVal?: undefined;
                } | {
                    propertyName: string;
                    modelName: string;
                    defaultVal: string;
                } | {
                    propertyName: string;
                    modelName: string;
                    defaultVal: boolean;
                })[];
            }
            class ColumnExpression {
                private _criteria;
                private _dependedTables;
                constructor(model: any, query: QueryViewModel, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
                table: ko.Computed<string>;
                column: ko.Observable<string> | ko.Computed<string>;
                expression: ko.Observable<string> | ko.Computed<string>;
                aggregate: ko.Observable<string> | ko.Computed<string>;
                alias: ko.Observable<string> | ko.Computed<string>;
                descending: ko.Observable<boolean> | ko.Computed<boolean>;
                itemType: ko.Observable<string> | ko.Computed<string>;
                actualName(): string;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                isDepended(tableActualName: string): boolean;
            }
            class ColumnSurface extends Analytics.Elements.SurfaceElementBase<ColumnViewModel> {
                private _isJoined;
                private _isHovered;
                constructor(control: ColumnViewModel, context: Analytics.Elements.ISurfaceContext);
                template: string;
                toggleSelected: () => void;
                selectedWrapper: ko.PureComputed<boolean>;
                isAggregate: ko.PureComputed<boolean>;
                isAscending: ko.PureComputed<boolean>;
                isDescending: ko.PureComputed<boolean>;
                cssClasses: (query: QuerySurface, columnDragHandler: {
                    getDragColumn: () => ColumnViewModel;
                }, parent: TableSurface) => {
                    'dxd-state-active': boolean;
                    'dxd-state-joined': ko.Computed<boolean>;
                    'dxd-state-hovered': ko.Computed<boolean>;
                };
            }
            module Metadata {
                var ConditionType: {
                    Equal: string;
                    NotEqual: string;
                    Greater: string;
                    GreaterOrEqual: string;
                    Less: string;
                    LessOrEqual: string;
                };
                var joinConditionSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class JoinConditionViewModel extends Analytics.Diagram.RoutedConnectorViewModel {
                getControlFactory(): Analytics.Utils.ControlsFactory;
                preInitProperties(): void;
                constructor(control: any, relation: RelationViewModel, serializer?: DevExpress.Analytics.Utils.ModelSerializer);
                parentColumn: ko.Computed<ColumnViewModel>;
                nestedColumn: ko.Computed<ColumnViewModel>;
                parentColumnName: ko.Observable<string> | ko.Computed<string>;
                nestedColumnName: ko.Observable<string> | ko.Computed<string>;
                operator: ko.Observable<string> | ko.Computed<string>;
                joinType: ko.Observable<string> | ko.Computed<string>;
                left: ko.Computed<string>;
                right: ko.Computed<string>;
            }
            class JoinConditionSurface extends Analytics.Diagram.RoutedConnectorSurface {
                constructor(control: JoinConditionViewModel, context: Analytics.Elements.ISurfaceContext);
                container(): QuerySurface;
            }
            module Metadata {
                var ParametersMode: {
                    ReadWrite: string;
                    Read: string;
                    Disabled: string;
                };
            }
            class ParameterViewModel extends Analytics.Data.DataSourceParameter {
                getEditorType(type: any): {
                    header?: any;
                    content?: any;
                    custom?: any;
                };
            }
            class QueryElementBaseSurface<M extends QueryElementBaseViewModel> extends Analytics.Elements.SurfaceElementBase<M> {
                static _unitProperties: Analytics.Internal.IUnitProperties<QueryElementBaseViewModel>;
                constructor(control: M, context: Analytics.Elements.ISurfaceContext, unitProperties: Analytics.Internal.IUnitProperties<M>);
                template: string;
                selectiontemplate: string;
                contenttemplate: string;
                margin: ko.Observable<number>;
            }
            class QueryViewModel extends QueryElementBaseViewModel {
                private static pageMargin;
                private static emptyModel;
                private _initializeTable;
                constructor(querySource: any, dbSchemaProvider: Analytics.Data.IDBSchemaProvider, parametersMode?: string, serializer?: DevExpress.Analytics.Utils.ModelSerializer);
                isPropertyDisabled(name: string): boolean;
                isValid: ko.Computed<boolean>;
                editableName: ko.Observable<string> | ko.Computed<string>;
                filterString: Widgets.Internal.QBFilterStringOptions;
                _filterString: ko.Observable<string> | ko.Computed<string>;
                groupFilterString: Widgets.Internal.QBFilterStringOptions;
                _groupFilterString: ko.Observable<string> | ko.Computed<string>;
                top: ko.Observable<number> | ko.Computed<number>;
                skip: ko.Observable<number> | ko.Computed<number>;
                pageWidth: ko.Observable<number> | ko.Computed<number>;
                pageHeight: ko.Observable<number> | ko.Computed<number>;
                relations: ko.ObservableArray<RelationViewModel>;
                tables: ko.ObservableArray<TableViewModel>;
                columns: ko.ObservableArray<ColumnExpression>;
                filter: ko.Observable<string> | ko.Computed<string>;
                parameters: ko.ObservableArray<ParameterViewModel> | ko.Computed<ParameterViewModel[]>;
                margins: Analytics.Elements.Margins;
                dbSchemaProvider: Analytics.Data.IDBSchemaProvider;
                allColumnsInTablesSelected: ko.Observable<boolean> | ko.Computed<boolean>;
                sorting: ko.ObservableArray<ColumnExpression>;
                grouping: ko.ObservableArray<ColumnExpression>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                createChild(info: any): Analytics.Elements.ElementViewModel;
                aggregatedColumnsCount: ko.Observable<number>;
                getAllColumns(): ColumnViewModel[];
                tryToCreateRelationsByFK(sourceTable: TableViewModel): void;
                addChild(control: Analytics.Elements.ElementViewModel): void;
                removeChild(control: Analytics.Elements.ElementViewModel): void;
                getTable(name: string): TableViewModel;
                private _findTableInAncestors;
                private _findHead;
                private _isHead;
                private _findAncestorsRelations;
                private _reverseRelations;
                cerateJoinCondition(parentColumn: ColumnViewModel, nestedColumn: ColumnViewModel): JoinConditionViewModel;
                private _validate;
                private _validateTable;
                serialize(includeRootTag?: boolean): any;
                save(): any;
                onSave: (data: any) => void;
            }
            module Metadata {
                var querySerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class QuerySurface extends Analytics.Elements.SurfaceElementBase<QueryViewModel> implements Analytics.Internal.ISelectionTarget, Analytics.Elements.ISurfaceContext {
                static _unitProperties: Analytics.Internal.IUnitProperties<QueryViewModel>;
                private _joinedColumns;
                constructor(query: QueryViewModel, zoom?: ko.Observable<number>);
                measureUnit: ko.Observable<string> | ko.Computed<string>;
                dpi: ko.Observable<number> | ko.Computed<number>;
                zoom: ko.Observable<number> | ko.Computed<number>;
                placeholder: () => any;
                tables: ko.ObservableArray<TableSurface>;
                relations: ko.ObservableArray<RelationSurface>;
                allowMultiselect: boolean;
                focused: ko.Observable<boolean>;
                selected: ko.Observable<boolean>;
                underCursor: ko.Observable<Analytics.Internal.IHoverInfo>;
                checkParent(surfaceParent: Analytics.Internal.ISelectionTarget): boolean;
                pageWidth: ko.Observable<number> | ko.Computed<number>;
                templateName: string;
                getChildrenCollection(): ko.ObservableArray<TableSurface>;
                margins: Analytics.Elements.IMargins;
                rtl: ko.Observable<boolean>;
                isJoined(column: ColumnSurface): boolean;
            }
            module Metadata {
                var relationSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class RelationViewModel extends QueryElementBaseViewModel {
                private _getConditionNumber;
                constructor(model: any, query: QueryViewModel, serializer?: DevExpress.Analytics.Utils.ModelSerializer);
                parentTableName: ko.Observable<string> | ko.Computed<string>;
                nestedTableName: ko.Observable<string> | ko.Computed<string>;
                parentTable: ko.Observable<TableViewModel>;
                nestedTable: ko.Observable<TableViewModel>;
                joinType: ko.Observable<string> | ko.Computed<string>;
                conditions: ko.ObservableArray<JoinConditionViewModel>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                addChild(control: Analytics.Elements.IElementViewModel): void;
                removeChild(control: Analytics.Elements.ElementViewModel): void;
            }
            class RelationSurface extends Analytics.Elements.SurfaceElementBase<RelationViewModel> {
                constructor(control: RelationViewModel, context: Analytics.Elements.ISurfaceContext);
                conditions: ko.ObservableArray<JoinConditionSurface>;
                template: string;
                _getChildrenHolderName(): string;
            }
            module Metadata {
                var tableSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class TableViewModel extends QueryElementBaseViewModel {
                private serializer?;
                static COLUMNS_OFFSET: number;
                static COLUMN_HEIGHT: number;
                static COLUMN_MARGIN: number;
                static TABLE_MIN_WIDTH: number;
                static TABLE_DEFAULT_HEIGHT: number;
                private _columnsConnectionPointLeftX;
                private _columnsConnectionPointRightX;
                private _columns;
                private _initialized;
                constructor(model: any, parent: QueryViewModel, serializer?: DevExpress.Analytics.Utils.ModelSerializer);
                size: Analytics.Elements.Size;
                location: Analytics.Elements.Point;
                name: ko.Observable<string> | ko.Computed<string>;
                alias: ko.Observable<string> | ko.Computed<string>;
                actualName: ko.PureComputed<string>;
                isReady: ko.Observable<boolean>;
                columns(): ColumnViewModel[];
                asterisk: AllColumnsViewModel;
                allColumnsSelected: ko.Computed<boolean>;
                toggleSelectedColumns(): void;
                isInitialized: ko.PureComputed<boolean>;
                getColumnConnectionPoints(column: ColumnViewModel): {
                    left: Analytics.Elements.IPoint;
                    right: Analytics.Elements.IPoint;
                };
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                getColumn(name: string): ColumnViewModel;
                createColumns(dbTable: Analytics.Data.DBTable): void;
            }
            class TableSurface extends QueryElementBaseSurface<TableViewModel> {
                constructor(control: TableViewModel, context: Analytics.Elements.ISurfaceContext);
                columnsAsyncResolver: Analytics.Internal.CodeResolver;
                asterisk: AllColumnsSurface;
                columns: ko.Computed<ColumnSurface[]>;
                contenttemplate: string;
                template: string;
                isInitialized: ko.Computed<boolean>;
                toggleSelected: () => void;
                selectedWrapper: ko.PureComputed<boolean>;
                resizable(resizeHandler: any, element: any): any;
            }
        }
        module Internal {
            class ColumnDragHandler extends Analytics.Internal.DragDropHandler {
                private querySurface;
                private undoEngine;
                private _dragColumn;
                private _dragConditionSurface;
                private _needToCreateRelation;
                constructor(querySurface: ko.Observable<Elements.QuerySurface>, selection: Analytics.Internal.SurfaceSelection, undoEngine: ko.Observable<Analytics.Utils.UndoEngine>, snapHelper: Analytics.Internal.SnapLinesHelper, dragHelperContent: Analytics.Internal.DragHelperContent);
                startDrag(control: Analytics.Internal.ISelectionTarget): void;
                setConnectorPoints(cursorPosition: {
                    top: number;
                    left: number;
                }): void;
                drag(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
                doStopDrag(): void;
                dragDropConnector: ko.Observable<Analytics.Diagram.RoutedConnectorSurface>;
                getDragColumn(): Elements.ColumnViewModel;
            }
            class DbObjectDragDropHandler extends Analytics.Internal.DragDropHandler {
                private _undoEngine;
                private _querySurface;
                private _drop;
                private _query;
                static getDropCallback: (undoEngine: ko.Observable<Analytics.Utils.UndoEngine>, suggestLocation: boolean) => (memberInfo: Analytics.Utils.IDataMemberInfo, query: Elements.QueryViewModel) => Elements.TableViewModel;
                constructor(surface: ko.Observable<Elements.QuerySurface>, selection: Analytics.Internal.SurfaceSelection, _undoEngine: ko.Observable<Analytics.Utils.UndoEngine>, snapHelper: Analytics.Internal.SnapLinesHelper, dragHelperContent: Analytics.Internal.DragHelperContent);
                startDrag(draggable: any): void;
                doStopDrag(ui: any, draggable: any): void;
                addControl(control: any, dropTargetSurface: any, size: any): void;
            }
        }
    }
}

/**
* DevExpress HTML/JS Reporting (dx-webdocumentviewer.d.ts)
* Version: 19.1.6
* Build date: 2019-09-10
* Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
* License: https://www.devexpress.com/Support/EULAs/NetComponents.xml
*/

declare module DevExpress.Reporting {
    module Internal {
        var editorTemplates: {
            csvSeparator: {
                header: any;
                extendedOptions: {
                    placeholder: ko.PureComputed<string>;
                };
            };
        };
    }
    module Metadata {
        var previewBackColor: Analytics.Utils.ISerializationInfo;
        var previewSides: Analytics.Utils.ISerializationInfo;
        var previewBorderColor: Analytics.Utils.ISerializationInfo;
        var previewBorderStyle: Analytics.Utils.ISerializationInfo;
        var previewBorderDashStyle: Analytics.Utils.ISerializationInfo;
        var previewBorderWidth: Analytics.Utils.ISerializationInfo;
        var previewForeColor: Analytics.Utils.ISerializationInfo;
        var previewFont: Analytics.Utils.ISerializationInfo;
        var previewPadding: Analytics.Utils.ISerializationInfo;
        var previewTextAlignment: Analytics.Utils.ISerializationInfo;
        var brickStyleSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
    }
    class ImageSource {
        sourceType: string;
        data: string;
        constructor(sourceType: string, data: string);
        getDataUrl(format?: ko.Observable<string>): string;
        static parse(val: string): ImageSource;
        static toString(val: ImageSource): string;
    }
    interface IKeyValuePair<T> {
        Key: string;
        Value: T;
    }
    function convertMapToKeyValuePair(object: any): any[];
    interface IEnumType {
        enumType: string;
        values: Array<IEnumValue>;
    }
    interface IEnumValue {
        displayName: string;
        name: string;
        value: any;
    }
    class CustomizeExportOptionsEventArgs {
        constructor(options: Viewer.Utils.IPreviewExportOptionsCustomizationArgs);
        HideExportOptionsPanel(): void;
        HideFormat(format: any): void;
        HideProperties(format: any, ...paths: (string | string[])[]): void;
        GetExportOptionsModel(format: any): any;
        _options: Viewer.Utils.IPreviewExportOptionsCustomizationArgs;
    }
    class EventGenerator {
        static generateCustomizeLocalizationCallback(fireEvent: (eventName: any, args?: any) => void): (localizationCallbacks: JQueryPromise<any>[]) => void;
        static generateDesignerEvents(fireEvent: (eventName: any, args?: any) => void): {
            customizeActions: (actions: any) => void;
            customizeParameterEditors: (parameter: any, info: any) => void;
            customizeParameterLookUpSource: (parameter: any, items: any) => any;
            exitDesigner: () => void;
            reportSaving: (args: any) => void;
            reportSaved: (args: any) => void;
            reportOpening: (args: any) => void;
            reportOpened: (args: any) => void;
            tabChanged: (tab: any) => void;
            onServerError: (args: any) => void;
            customizeParts: (parts: any) => void;
            componentAdded: (args: any) => void;
            customizeSaveDialog: (popup: any) => void;
            customizeSaveAsDialog: (popup: any) => void;
            customizeOpenDialog: (popup: any) => void;
            customizeToolbox: (controlsFactory: any) => void;
            customizeLocalization: (localizationCallbacks: JQueryPromise<any>[]) => void;
            customizeFieldListActions: (item: any, actions: any) => void;
            beforeRender: (designerModel: any) => void;
            customizeWizard: (type: string, wizard: any) => void;
        };
        static generatePreviewEvents(fireEvent: (eventName: any, args?: any) => void, prefix?: string): {
            previewClick: (pageIndex: any, brick: any, defaultHandler: any) => boolean;
            documentReady: (documentId: any, reportId: any, pageCount: any) => void;
            editingFieldChanged: (field: any, oldValue: any, newValue: any) => any;
            parametersSubmitted: (model: any, parameters: any) => void;
            parametersReset: (model: any, parameters: any) => void;
            customizeParameterLookUpSource: (parameter: any, items: any) => any;
            customizeParameterEditors: (parameter: any, info: any) => void;
            customizeActions: (actions: any) => void;
            customizeParts: (parts: any) => void;
            customizeExportOptions: (options: any) => void;
            onServerError: (args: any) => void;
        };
    }
    module Internal {
        var cultureInfo: {};
        var generateGuid: () => string;
        function createFullscreenComputed(element: Element, parent: Analytics.Utils.IDisposable): ko.Computed<boolean>;
    }
    module Editing {
        interface IInplaceEditorInfo {
            name: string;
            category: string;
            displayName: string;
            template?: string;
            options?: {};
        }
        var Categories: {
            Image: () => string;
            Numeric: () => string;
            DateTime: () => string;
            Letters: () => string;
        };
        interface IImageEditorRegistrationOptions {
            name: string;
            displayName: string;
            images?: Viewer.Widgets.Internal.IImageEditorItem[];
            customizeActions?: (sender: any, actions: any[]) => void;
            searchEnabled?: boolean;
            imageLoadEnabled?: boolean;
            sizeOptionsEnabled?: boolean;
            clearEnabled?: boolean;
            drawingEnabled?: boolean;
        }
        class EditingFieldExtensions {
            private static _instance;
            private _editors;
            static instance(): EditingFieldExtensions;
            private _registerStandartEditors;
            static registerImageEditor(imageRegistrationOptions: IImageEditorRegistrationOptions): void;
            static registerEditor(name: string, displayName: string, category: string, options?: {}, template?: string, validate?: (value: string) => boolean, defaultVal?: string): void;
            static registerMaskEditor(editorID: string, displayName: string, category: string, mask: string): void;
            static registerRegExpEditor(editorID: string, displayName: string, category: string, regExpEditing: RegExp, regExpFinal: RegExp, defaultVal: string): void;
            static unregisterEditor(editorID: string): void;
            categories(excludeCategories?: string[]): string[];
            editors(): IInplaceEditorInfo[];
            editorsByCategories(categories?: string[]): IInplaceEditorInfo[];
            editor(editorID: string): IInplaceEditorInfo;
        }
    }
    module Export {
        module Metadata {
            var pageBorderColor: Analytics.Utils.ISerializationInfo;
            var pageBorderWidth: Analytics.Utils.ISerializationInfo;
            var pageRange: Analytics.Utils.ISerializationInfo;
            var expotOptionsTitle: Analytics.Utils.ISerializationInfo;
            var htmlTableLayout: Analytics.Utils.ISerializationInfo;
            var docxTableLayout: Analytics.Utils.ISerializationInfo;
            var allowURLsWithJSContent: Analytics.Utils.ISerializationInfo;
            var rasterizationResolution: Analytics.Utils.ISerializationInfo;
            var rasterizeImages: Analytics.Utils.ISerializationInfo;
            var useHRefHyperlinks: Analytics.Utils.ISerializationInfo;
            var exportWatermarks: Analytics.Utils.ISerializationInfo;
            var inlineCss: Analytics.Utils.ISerializationInfo;
            var removeSecondarySymbols: Analytics.Utils.ISerializationInfo;
            var characterSet: Analytics.Utils.ISerializationInfo;
            function getExportModeValues(format?: string, preview?: boolean, merged?: boolean): Array<Analytics.Utils.IDisplayedValue>;
            var exportPageBreaks: Analytics.Utils.ISerializationInfo;
            var rtfExportMode: Analytics.Utils.ISerializationInfo;
            var docxExportMode: Analytics.Utils.ISerializationInfo;
            var htmlExportMode: Analytics.Utils.ISerializationInfo;
            var embedImagesInHTML: Analytics.Utils.ISerializationInfo;
            var imageExportMode: Analytics.Utils.ISerializationInfo;
            var xlsExportMode: Analytics.Utils.ISerializationInfo;
            var xlsxExportMode: Analytics.Utils.ISerializationInfo;
            var textExportMode: Analytics.Utils.ISerializationInfo;
            var xlsTextExportMode: Analytics.Utils.ISerializationInfo;
            var csvTextSeparator: Analytics.Utils.ISerializationInfo;
            var useCustomSeparator: Analytics.Utils.ISerializationInfo;
            var textEncodingType: Analytics.Utils.ISerializationInfo;
            var xlsExportHyperlinks: Analytics.Utils.ISerializationInfo;
            var xlsRawDataMode: Analytics.Utils.ISerializationInfo;
            var xlsShowGridLines: Analytics.Utils.ISerializationInfo;
            var xlsExportOptionsSheetName: Analytics.Utils.ISerializationInfo;
        }
        class CsvExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): CsvExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            useCustomSeparator: ko.Observable<boolean> | ko.Computed<boolean>;
            separator: ko.Observable<string> | ko.Computed<string>;
            defaultSeparatorValue: string;
        }
        module Metadata {
            var csvExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class TextExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): TextExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
        }
        module Metadata {
            var textExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class RtfExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): RtfExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            rtfExportMode: ko.Observable<string> | ko.Computed<string>;
        }
        module Metadata {
            var rtfExportOptionsSerializationInfoBase: Analytics.Utils.ISerializationInfoArray;
            var emptyFirstPageHeaderFooter: Analytics.Utils.ISerializationInfo;
            var keepRowHeight: Analytics.Utils.ISerializationInfo;
            var rtfExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class DocxExportDocumentOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): DocxExportDocumentOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
        }
        class DocxExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): DocxExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            docxExportMode: ko.Observable<string> | ko.Computed<string>;
            tableLayout: ko.Observable<boolean> | ko.Computed<boolean>;
        }
        module Metadata {
            var docxExportDocumentOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            var docxDocumentOptions: Analytics.Utils.ISerializationInfo;
            var docxExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class HtmlExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): HtmlExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            htmlExportMode: ko.Observable<string> | ko.Computed<string>;
        }
        module Metadata {
            var htmlExportOptionsSerializationInfoBase: Analytics.Utils.ISerializationInfoArray;
            var htmlExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class ImageExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): ImageExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            imageExportMode: ko.Observable<string> | ko.Computed<string>;
        }
        module Metadata {
            var imageExportOptionsSerializationInfoBase: Analytics.Utils.ISerializationInfoArray;
            var imageExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class MhtExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): MhtExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            htmlExportMode: ko.Observable<string> | ko.Computed<string>;
        }
        module Metadata {
            var mhtExportOptionsSerializationInfoBase: Analytics.Utils.ISerializationInfoArray;
            var mhtExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class PdfExportDocumentOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): PdfExportDocumentOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
        }
        class PdfPermissionsOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): PdfPermissionsOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
        }
        class PdfPasswordSecurityOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): PdfPasswordSecurityOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            hasSensitiveData(): boolean;
            openPassword: ko.Observable<string> | ko.Computed<string>;
            permissionsPassword: ko.Observable<string> | ko.Computed<string>;
        }
        class PdfExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): PdfExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            isPropertyDisabled(propertyName: string): boolean;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            hasSensitiveData(): boolean;
            pdfACompatibility: ko.Observable<string> | ko.Computed<string>;
            pdfPasswordSecurityOptions: PdfPasswordSecurityOptions;
        }
        module Metadata {
            var author: Analytics.Utils.ISerializationInfo;
            var application: Analytics.Utils.ISerializationInfo;
            var title: Analytics.Utils.ISerializationInfo;
            var subject: Analytics.Utils.ISerializationInfo;
            var pdfExportDocumentOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            var pdfExportPermissionsOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            var pdfEncryptionLevel: Analytics.Utils.ISerializationInfo;
            var pdfExportPasswordSecurityOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            var pdfACompatibilityValues: {
                None: string;
                PdfA1b: string;
                PdfA2b: string;
                PdfA3b: string;
            };
            var pdfACompatibility: {
                propertyName: string;
                modelName: string;
                displayName: string;
                localizationId: string;
                editor: any;
                defaultVal: string;
                from: typeof Analytics.Utils.fromEnum;
                valuesArray: {
                    value: string;
                    displayValue: string;
                    localizationId: string;
                }[];
            };
            var pdfExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class AdditionalRecipientModel implements DevExpress.Analytics.Utils.ISerializableModel {
            static createNew: () => AdditionalRecipientModel;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
        }
        module Metadata {
            var nativeFormatOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            var additionalRecipientSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var additionalRecipients: Analytics.Utils.ISerializationInfo;
            var emailOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class PrintPreviewOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): PrintPreviewOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
        }
        module Metadata {
            var printPreviewOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class XlsExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): XlsExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            hasSensitiveData(): boolean;
            xlsExportMode: ko.Observable<string> | ko.Computed<string>;
            encryptionOptions: {
                password: ko.Observable<string>;
            };
        }
        module Metadata {
            var xlsExportOptionsSerializationInfoCommon: Analytics.Utils.ISerializationInfoArray;
            var xlsExportOptionsSerializationInfoBase: Analytics.Utils.ISerializationInfoArray;
            var xlsExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class XlsxExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): XlsxExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            isPropertyDisabled(name: string): boolean;
            hasSensitiveData(): boolean;
            xlsxExportMode: ko.Observable<string> | ko.Computed<string>;
            encryptionOptions: {
                password: ko.Observable<string>;
            };
        }
        module Metadata {
            var xlsxExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
        class ExportOptions {
            static from(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer): ExportOptions;
            static toJson(value: any, serializer: any, refs: any): any;
            getInfo(): Analytics.Utils.ISerializationInfoArray;
            constructor(model: any, serializer?: DevExpress.Analytics.Utils.IModelSerializer);
            csv: CsvExportOptions;
            html: HtmlExportOptions;
            image: ImageExportOptions;
            mht: MhtExportOptions;
            pdf: PdfExportOptions;
            printPreview: PrintPreviewOptions;
            rtf: RtfExportOptions;
            textExportOptions: TextExportOptions;
            xls: XlsExportOptions;
            xlsx: XlsxExportOptions;
            docx: DocxExportOptions;
        }
        module Metadata {
            var exportOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
        }
    }
    module Viewer {
        var ActionId: {
            Design: string;
            FirstPage: string;
            PrevPage: string;
            Pagination: string;
            NextPage: string;
            LastPage: string;
            MultipageToggle: string;
            HighlightEditingFields: string;
            ZoomOut: string;
            ZoomSelector: string;
            ZoomIn: string;
            Print: string;
            PrintPage: string;
            ExportTo: string;
            Search: string;
            FullScreen: string;
        };
        var ExportFormatID: {
            PDF: {
                text: string;
                textId: string;
                format: string;
            };
            XLS: {
                text: string;
                textId: string;
                format: string;
            };
            XLSX: {
                text: string;
                textId: string;
                format: string;
            };
            RTF: {
                text: string;
                textId: string;
                format: string;
            };
            MHT: {
                text: string;
                textId: string;
                format: string;
            };
            HTML: {
                text: string;
                textId: string;
                format: string;
            };
            Text: {
                text: string;
                textId: string;
                format: string;
                propertyName: string;
            };
            CSV: {
                text: string;
                textId: string;
                format: string;
            };
            Image: {
                text: string;
                textId: string;
                format: string;
            };
            DOCX: {
                text: string;
                textId: string;
                format: string;
            };
        };
        var PreviewElements: {
            Toolbar: string;
            Surface: string;
            RightPanel: string;
        };
        var ZoomAutoBy: {
            None: number;
            WholePage: number;
            PageWidth: number;
        };
        module Settings {
            var EditablePreviewEnabled: ko.Observable<boolean>;
            var SearchAvailable: ko.Observable<boolean>;
            var HandlerUri: string;
            var ReportServerDownloadUri: string;
            var ReportServerInvokeUri: string;
            var ReportServerExportUri: string;
            var TimeOut: number;
            var PollingDelay: number;
            var previewDefaultResolution: number;
            var AsyncExportApproach: boolean;
            var MessageHandler: IMessageHandler;
            interface IMessageHandler {
                processError: (message: string, showForUser?: boolean, prefix?: string) => void;
                processMessage: (message: string, showForUser?: boolean) => void;
                processWarning: (message: string, showForUser?: boolean) => void;
            }
        }
        module Internal {
            class PreviewDesignerActions extends Analytics.Utils.Disposable implements Analytics.Internal.IActionsProvider {
                actions: Analytics.Utils.IAction[];
                dispose(): void;
                constructor(reportPreview: ReportPreview, element: Element);
                getActions(context: any): Analytics.Utils.IAction[];
            }
            class ActionLists extends Analytics.Internal.ActionListsBase {
                private _reportPreview;
                constructor(reportPreview: ReportPreview, globalActionProviders: ko.ObservableArray<Analytics.Internal.IActionsProvider>, customizeActions?: (actions: Analytics.Utils.IAction[]) => void, enabled?: ko.Observable<boolean>);
                processShortcut(actions: Analytics.Utils.IAction[], e: JQueryKeyEventObject): void;
                dispose(): void;
                globalActionProviders: ko.ObservableArray<Analytics.Internal.IActionsProvider>;
            }
            class PreviewActions extends Analytics.Utils.Disposable implements Analytics.Internal.IActionsProvider {
                actions: Analytics.Utils.IAction[];
                wrapDisposable<T>(object: T): T;
                constructor(reportPreview: ReportPreview);
                dispose(): void;
                getActions(context: any): Analytics.Utils.IAction[];
            }
            function convertToPercent(childSize: any, parentSize: any): string;
            function brickText(brick: Utils.IBrickNode, editingFieldsProvider?: () => Editing.EditingField[]): any;
            function updateBricksPosition(brick: Utils.IBrickNode, height: any, width: any): void;
            function initializeBrick(brick: Utils.IBrickNode, processClick: (target: Utils.IBrickNode, e?: JQueryEventObject) => void, zoom: ko.Observable<number> | ko.Computed<number>, editingFieldBricks: Utils.IBrickNode[]): void;
            function getCurrentResolution(zoom: any): number;
            function getImageBase64(url: any): JQueryPromise<{}>;
            function getEnumValues(enumType: any): string[];
        }
        module Utils {
            interface IPreviewInitialize {
                reportId?: string;
                documentData?: Internal.IGeneratedDocumentData;
                reportUrl?: string;
                documentId?: string;
                pageSettings?: IPreviewPageInitialSettings;
                exportOptions?: string;
                parametersInfo?: Parameters.IReportParametersInfo;
                rtlReport?: boolean;
                error?: any;
            }
            interface IPreviewModel {
                tabPanel: Analytics.Utils.TabPanel;
                reportPreview: ReportPreview;
                Close: () => void;
                ExportTo: (format?: string, inlineResult?: boolean) => void;
                GetCurrentPageIndex: () => number;
                GetParametersModel: () => Parameters.PreviewParametersViewModel;
                GoToPage: (pageIndex: number) => void;
                OpenReport: (reportUrl: string) => void;
                Print: (pageIndex?: number) => JQueryPromise<boolean>;
                ResetParameters: () => void;
                StartBuild: () => void;
            }
            interface IPreviewPageInitialSettings {
                height?: number;
                width?: number;
                color?: string;
            }
            interface IParametersCustomizationHandler {
                customizeParameterEditors?: (parameter: Parameters.IParameterDescriptor, info: DevExpress.Analytics.Utils.ISerializationInfo) => void;
                customizeParameterLookUpSource?: (parameter: Parameters.IParameterDescriptor, items: Array<DevExpress.Analytics.Utils.IDisplayedValue>) => any;
                parametersReset?: (parametersViewModel: Parameters.PreviewParametersViewModel, parameters: Parameters.IParameter[]) => void;
                parametersSubmitted?: (parametersViewModel: Parameters.PreviewParametersViewModel, parameters: Array<IKeyValuePair<any>>) => void;
            }
            interface IPreviewCustomizationHandler extends IParametersCustomizationHandler, Analytics.Internal.ICommonCustomizationHandler {
                customizeParts?: (parts: Analytics.Internal.IDesignerPart[]) => void;
                previewClick?: (pageIndex: number, brick: Utils.IBrickNode, defaultHandler: () => void) => boolean;
                editingFieldChanged?: (field: Editing.EditingField, oldValue: any, newValue: any) => any;
                documentReady?: (documentId: string, reportId: string, pageCount: number) => void;
                customizeExportOptions?: (options: IPreviewExportOptionsCustomizationArgs) => void;
            }
            interface IPreviewExportOptionsCustomizationArgs {
                exportOptions: Export.ExportOptionsPreview;
                panelVisible: boolean;
            }
            interface IMobileModeSettings {
                readerMode?: boolean;
                animationEnabled?: boolean;
            }
            interface ITabPanelSettings {
                position?: string;
                width?: number;
            }
            interface IRemoteSettings {
                authToken?: string;
                serverUri?: string;
            }
            interface IWebDocumentViewerSettings extends DevExpress.Analytics.Internal.ILocalizationSettings {
                handlerUri?: string;
                allowURLsWithJSContent?: boolean;
                rtl?: boolean;
                isMobile?: boolean;
                mobileModeSettings?: IMobileModeSettings;
                remoteSettings?: IRemoteSettings;
                tabPanelSettings?: ITabPanelSettings;
            }
            interface IWebDocumentViewerModel extends IPreviewInitialize, IWebDocumentViewerSettings, Analytics.Internal.IGlobalizeSettings {
                cultureInfoList?: {
                    [key: string]: string;
                };
            }
            interface IBrickNode {
                top: number;
                topP: string;
                left: number;
                leftP?: string;
                rightP?: string;
                height: number;
                heightP: string;
                width: number;
                widthP: string;
                bricks: IBrickNode[];
                content: Array<IKeyValuePair<any>>;
                indexes: string;
                row: number;
                col: number;
                genlIndex: number;
                active: ko.Observable<boolean> | ko.Computed<boolean>;
                navigation?: IBrickNodeNavigation;
                rtl: boolean;
                efIndex?: number;
                absoluteBounds?: Editing.IBounds;
                text: () => string;
                onClick?: (args?: any) => void;
            }
            interface IBrickNodeNavigation {
                url?: string;
                target?: string;
                indexes?: string;
                pageIndex?: number;
                drillDownKey?: string;
                sortData?: ISortingData;
            }
            interface ISortingData {
                target: string;
                field: string;
                order: Internal.ColumnSortOrder;
            }
            interface IDocumentOperationResult {
                documentId: string;
                succeeded: boolean;
                message?: string;
                customData?: string;
            }
        }
        module Internal {
            interface IProgressStatus {
                requestAgain: boolean;
                completed?: boolean;
                progress?: number;
                error?: string;
            }
            interface IExportProgressStatus extends IProgressStatus {
                token?: string;
                uri?: string;
            }
            interface IDocumentBuildStatus extends IProgressStatus {
                pageCount?: number;
            }
            class PreviewHandlersHelper {
                private _preview;
                constructor(preview: ReportPreview);
                doneStartExportHandler(deffered: any, inlineResult: boolean, response: any, printable?: boolean): void;
                errorStartExportHandler(deffered: any, error: any): void;
                doneExportStatusHandler(deffered: JQueryDeferred<any>, operationId: string, response: any): void;
                errorExportStatusHandler(deffered: any, error: any): void;
                doneStartBuildHandler(deffered: any, response: any): void;
                errorStartBuildHandler(deffered: any, error: any, startBuildOperationId: any): void;
                errorGetBuildStatusHandler(deffered: JQueryDeferred<IDocumentBuildStatus>, error: any, ignoreError: () => boolean): void;
                doneGetBuildStatusHandler(deffered: JQueryDeferred<IDocumentBuildStatus>, documentId: string, response: any, stopProcessingPredicate: () => boolean): void;
            }
            interface IGetPageResponse extends IGetBrickMapResult {
                width: number;
                height: number;
                base64string: string;
            }
            interface IGetBrickMapResult {
                brick: Utils.IBrickNode;
                columnWidthArray: Array<number>;
            }
            enum ColumnSortOrder {
                None = 0,
                Ascending = 1,
                Descending = 2
            }
            interface ISortingFieldInfo {
                fieldName?: string;
                sortOrder?: ColumnSortOrder;
            }
            interface IGeneratedDocumentData {
                documentMap?: Internal.IBookmarkNode;
                drillDownKeys?: Array<IKeyValuePair<boolean>>;
                sortingState?: Array<IKeyValuePair<Array<ISortingFieldInfo>>>;
                exportOptions?: string;
                canPerformContinuousExport?: boolean;
                editingFields?: Array<Editing.IEditingFieldModel>;
            }
            class PreviewRequestWrapper implements Editing.IEditingFieldHtmlProvider {
                private _callbacks?;
                private _reportPreview;
                private _parametersModel;
                private _searchModel;
                constructor(handlers?: {
                    [key: string]: Function;
                }, _callbacks?: Utils.IPreviewCustomizationHandler);
                static getProcessErrorCallback(reportPreview?: ReportPreview, defaultErrorMessage?: string, showMessage?: boolean): (message: string, jqXHR: any, textStatus: any) => void;
                static getPage(url: any, ignoreError?: () => boolean): JQueryPromise<IGetPageResponse>;
                initialize(reportPreview: ReportPreview, parametersModel: Parameters.PreviewParametersViewModel, searchModel: SearchViewModel): void;
                findTextRequest(text: any): JQueryPromise<any>;
                stopBuild(id: any): void;
                sendCloseRequest(documentId: string, reportId?: string): void;
                startBuildRequest(): JQueryPromise<any>;
                getBuildStatusRequest(documentId: any, shouldIgnoreError: () => boolean): JQueryPromise<any>;
                getDocumentData(documentId: any, shouldIgnoreError: () => boolean): JQueryPromise<IGeneratedDocumentData>;
                customDocumentOperation(documentId: string, serializedExportOptions: string, editindFields: any[], customData: string, hideMessageFromUser?: boolean): JQueryPromise<Utils.IDocumentOperationResult>;
                openReport(reportName: any): any;
                drillThrough(customData: string): any;
                getStartExportOperation(arg: string): any;
                getExportStatusRequest(operationId: string): any;
                getEditingFieldHtml(value: string, editingFieldIndex: number): any;
            }
            class PreviewSelection {
                private _element;
                private _page;
                private _click;
                static started: boolean;
                static disabled: boolean;
                private _$element;
                private _$selectionContent;
                private _bodyEvents;
                private _startRect;
                private _updateSelectionContent;
                private _mouseMove;
                private _mouseUp;
                private _mouseDown;
                constructor(_element: HTMLElement, _page: PreviewPage, _click: (pageIndex: number) => void);
                private _dispose;
                dispose: () => void;
            }
            interface IProgressHandler {
                stop?: () => void;
                cancelText?: ko.Observable<string> | ko.Computed<string>;
                progress: ko.Observable<number> | ko.Computed<number>;
                text: ko.Observable<string> | ko.Computed<string>;
                visible: ko.Observable<boolean> | ko.Computed<boolean>;
                startProgress: (onComplete?: () => void, onStop?: () => void) => void;
                inProgress: ko.Observable<boolean> | ko.Computed<boolean>;
                complete: () => void;
            }
            class ProgressViewModel implements IProgressHandler {
                progress: ko.Observable<number>;
                private _forceInvisible;
                private _onComplete;
                stop: () => void;
                inProgress: ko.Observable<boolean>;
                cancelText: ko.Observable<any>;
                text: ko.Observable<string>;
                visible: ko.PureComputed<boolean>;
                complete: () => void;
                startProgress: (onComplete?: () => void, onStop?: () => void) => void;
            }
            function updatePreviewContentSize(previewSize: ko.Observable<number> | ko.Computed<number>, root: Element, rtl?: boolean): (tabPanelPosition?: string) => void;
            function updatePreviewZoomWithAutoFit(width: any, height: any, $element: JQuery, autoFitBy?: number): number;
            class SortingProcessor {
                private _getSortingStage;
                constructor(_getSortingStage: () => Array<IKeyValuePair<Array<ISortingFieldInfo>>>);
                doSorting(sortData: Utils.ISortingData, shiftKey?: boolean, ctrlKey?: boolean): boolean;
                private _applySorting;
                private _appendSorting;
                private _detachSorting;
                private _changeSortOrder;
            }
            interface IPreviewPageOwner {
                _pageWidth: any;
                _pageHeight: any;
                _zoom: ko.Observable<number> | ko.Computed<number>;
                _currentDocumentId: ko.Observable<string> | ko.Computed<string>;
                _unifier: ko.Observable<string> | ko.Computed<string>;
                _pageBackColor?: ko.Observable<string> | ko.Computed<string>;
                _editingFields: ko.Observable<Editing.EditingField[]>;
                loading?: ko.Observable<boolean> | ko.Computed<boolean>;
                processClick?: (target: Utils.IBrickNode) => void;
                _closeDocumentRequests?: {
                    [key: string]: boolean;
                };
            }
            class PreviewPage extends Analytics.Utils.Disposable {
                private _initializeEditingFields;
                private _getPixelRatio;
                private _onPageLoaded;
                private _onPageLoadFailed;
                constructor(preview: IPreviewPageOwner, pageIndex: number, processClick?: (target: Utils.IBrickNode) => void, loading?: ko.Observable<boolean>);
                updateSize(zoom?: number): number;
                clearBricks(): void;
                _setPageImgSrc(documentId: string, unifier: string, zoom?: number): void;
                _clear(): void;
                initializeBrick(brick: Utils.IBrickNode, processClick: (target: Utils.IBrickNode) => void, zoom: ko.Observable<number> | ko.Computed<number>, editingFieldBricks: Utils.IBrickNode[]): void;
                clickToBrick(s: PreviewPage, e: JQueryEventObject): void;
                getBricksFlatList(brick: Utils.IBrickNode): Utils.IBrickNode[];
                editingFields: ko.Computed<Editing.IEditingFieldViewModel[]>;
                isClientVisible: ko.Observable<boolean>;
                documentId: ko.Observable<string> | ko.Computed<string>;
                originalHeight: ko.Observable<number>;
                originalWidth: ko.Observable<number>;
                selectBrick: (path: string, ctrlKey?: boolean) => void;
                resetBrickRecusive: (brick: Utils.IBrickNode) => void;
                getBricks: (pageIndex: number) => void;
                loadingText: string;
                realZoom: ko.Observable<number>;
                actualResolution: number;
                isEmpty: boolean;
                pageIndex: number;
                pageLoading: ko.Observable<boolean> | ko.Computed<boolean>;
                currentScaleFactor: ko.Observable<number>;
                zoom: ko.Observable<number> | ko.Computed<number>;
                width: ko.Observable<number> | ko.Computed<number>;
                height: ko.Observable<number> | ko.Computed<number>;
                color: string;
                imageHeight: ko.Observable<number>;
                imageWidth: ko.Observable<number>;
                imageSrc: ko.Observable<string>;
                displayImageSrc: ko.Observable<string>;
                brick: ko.Observable<Utils.IBrickNode>;
                brickLoading: ko.Observable<boolean>;
                brickColumnWidthArray: Array<number>;
                bricks: ko.Computed<Utils.IBrickNode[]>;
                activeBricks: ko.Computed<Utils.IBrickNode[]>;
                clickableBricks: ko.Computed<Utils.IBrickNode[]>;
                active: ko.Observable<boolean>;
                maxZoom: number;
                disableResolutionReduction: boolean;
                private _lastZoom;
                private _selectedBrickPath;
            }
            interface IPreviewModel {
                rootStyle: string | {
                    [key: string]: boolean;
                };
                reportPreview: ReportPreview;
                parametersModel: Parameters.PreviewParametersViewModel;
                exportModel: Export.ExportOptionsModel;
                searchModel: Internal.SearchViewModel;
                documentMapModel: Internal.DocumentMapModel;
                tabPanel: Analytics.Utils.TabPanel;
                actionLists: Internal.ActionLists;
                rtl: boolean;
                parts?: Analytics.Internal.IDesignerPart[];
                resizeCallback?: () => void;
                updateSurfaceSize?: () => void;
            }
            class PreviewModel extends Analytics.Utils.Disposable implements IPreviewModel {
                rootStyle: string | {
                    [key: string]: boolean;
                };
                reportPreview: ReportPreview;
                parametersModel: Parameters.PreviewParametersViewModel;
                exportModel: Export.ExportOptionsModel;
                searchModel: SearchViewModel;
                documentMapModel: DocumentMapModel;
                tabPanel: Analytics.Utils.TabPanel;
                actionLists: ActionLists;
                rtl: boolean;
                parts?: Analytics.Internal.IDesignerPart[];
                resizeCallback?: () => void;
                updateSurfaceSize?: () => void;
                constructor(options: IPreviewModel);
                _addDisposable(object: any): void;
                dispose(): void;
            }
            function createDesktopPreview(element: Element, callbacks: Utils.IPreviewCustomizationHandler, parametersInfo?: Parameters.IReportParametersInfo, handlerUri?: string, previewVisible?: boolean, applyBindings?: boolean, allowURLsWithJSContent?: boolean, rtl?: boolean, tabPanelSettings?: Utils.ITabPanelSettings): PreviewModel;
            function createPreview(element: Element, callbacks: Utils.IPreviewCustomizationHandler, localization?: Analytics.Internal.ILocalizationSettings, parametersInfo?: Parameters.IReportParametersInfo, handlerUri?: string, previewVisible?: boolean, rtl?: boolean, isMobile?: boolean, mobileModeSettings?: Utils.IMobileModeSettings, applyBindings?: boolean, allowURLsWithJSContent?: boolean, remoteSettings?: Utils.IRemoteSettings, tabPanelSettings?: Utils.ITabPanelSettings): JQueryDeferred<any>;
            function createAndInitPreviewModel(viewerModel: Utils.IWebDocumentViewerModel, element: Element, callbacks?: Utils.IPreviewCustomizationHandler, applyBindings?: boolean): JQueryDeferred<any>;
        }
        module Parameters {
            class MultiValuesHelper {
                items: Array<Analytics.Utils.IDisplayedValue>;
                constructor(value: ko.ObservableArray<any>, items: Array<Analytics.Utils.IDisplayedValue>);
                selectedItems: ko.ObservableArray<any>;
                isSelectedAll: ko.Observable<boolean> | ko.Computed<boolean>;
                maxDisplayedTags: number;
                dataSource: any;
                value: ko.ObservableArray<any>;
            }
            interface IParameter {
                getParameterDescriptor: () => IParameterDescriptor;
                value: ko.Observable | ko.Computed;
                type: any;
                isMultiValue: any;
                allowNull: any;
                multiValueInfo: ko.Observable<Analytics.Utils.ISerializationInfo> | ko.Computed<Analytics.Utils.ISerializationInfo>;
                tag?: any;
            }
            interface IParameterDescriptor {
                description: string;
                name: string;
                type: string;
                value: any;
                visible: boolean;
                multiValue?: boolean;
                allowNull?: boolean;
                tag?: any;
            }
            function getEditorType(typeString: any): any;
            class ParameterHelper {
                private _knownEnums;
                private _customizeParameterEditors;
                private _isKnownEnumType;
                static getSerializationValue(value: any, dateConverter: any): any;
                static createDefaultDataSource(store: ArrayStore): DataSource;
                initialize(knownEnums?: Array<DevExpress.Reporting.IEnumType>, callbacks?: Utils.IParametersCustomizationHandler): void;
                createInfo(parameter: IParameter): DevExpress.Analytics.Utils.ISerializationInfo;
                addShowCleanButton(info: DevExpress.Analytics.Utils.ISerializationInfo, parameter: IParameter): void;
                assignValueStore(info: DevExpress.Analytics.Utils.ISerializationInfo, parameter: IParameter): void;
                createMultiValue(parameter: IParameter, value?: any): {
                    value: ko.Observable<any>;
                    getInfo: () => Analytics.Utils.ISerializationInfo[];
                };
                createMultiValueArray(fromArray: Array<any>, parameter: IParameter, convertSingleValue?: (val: any) => any): ko.ObservableArray<{
                    value: ko.Observable<any>;
                    getInfo: () => Analytics.Utils.ISerializationInfo[];
                }>;
                isEnumType(parameter: IParameter): boolean;
                getItemsSource(parameterDescriptor: IParameterDescriptor, items: Array<Analytics.Utils.IDisplayedValue>, sort?: boolean): any;
                getEnumCollection(parameter: IParameter): Array<Analytics.Utils.IDisplayedValue>;
                getParameterInfo(parameter: IParameter): DevExpress.Analytics.Utils.ISerializationInfo;
                getValueConverter(type: string): (val: any) => any;
                customizeParameterLookUpSource: (parameter: IParameterDescriptor, items: Array<DevExpress.Analytics.Utils.IDisplayedValue>) => any;
                getUnspecifiedDisplayText: () => any;
            }
            interface IPreviewParameterDescriptor extends IParameterDescriptor {
                hasLookUpValues?: boolean;
            }
            class PreviewParameter extends Analytics.Utils.Disposable implements IParameter {
                static _compareValues(value1: any, value2: any): boolean;
                constructor(parameterInfo: IPreviewParameterInfo, parameterHelper: PreviewParameterHelper);
                getParameterDescriptor: () => IParameterDescriptor;
                safeAssignObservable(name: string, value: ko.Observable<any>): void;
                initialize(value: any, parameterHelper: PreviewParameterHelper): void;
                valueInfo: ko.Observable<Analytics.Utils.ISerializationInfo>;
                value: ko.Observable<any>;
                _value: ko.Observable<any>;
                _originalLookUpValues: Array<Analytics.Utils.IDisplayedValue>;
                _originalValue: any;
                tag: any;
                type: string;
                path: string;
                isFilteredLookUpSettings: boolean;
                lookUpValues: ko.ObservableArray<Analytics.Utils.IDisplayedValue>;
                valueStoreCache: any;
                allowNull: boolean;
                isMultiValue: boolean;
                isMultiValueWithLookUp: boolean;
                multiValueInfo: ko.Observable<Analytics.Utils.ISerializationInfo>;
                visible: boolean;
                intTypes: string[];
                floatTypes: string[];
                isTypesCurrentType: (types: string[], type: string) => boolean;
            }
            interface IReportParametersInfo {
                shouldRequestParameters?: boolean;
                parameters?: Array<IPreviewParameterInfo>;
                knownEnums?: Array<Reporting.IEnumType>;
            }
            interface IPreviewParameterInfo {
                Path: string;
                Description: string;
                Name: string;
                Value: any;
                TypeName: string;
                ValueInfo?: any;
                MultiValue?: boolean;
                AllowNull?: boolean;
                IsFilteredLookUpSettings?: boolean;
                LookUpValues?: Array<ILookUpValue>;
                Visible?: boolean;
                Tag?: any;
            }
            interface ILookUpValue {
                Description: string;
                Value: any;
            }
            class PreviewParametersViewModel extends Analytics.Utils.Disposable {
                private _parameters;
                private readonly _visibleParameters;
                private _shouldProcessParameter;
                private _reportPreview;
                private _convertLocalDateToUTC;
                private _getLookUpValueRequest;
                private _getDoneGetLookUpValueHandler;
                private _add;
                private _getFailGetLookUpValueHandler;
                private _setLookUpValues;
                private _getParameterValuesContainedInLookups;
                private _filterParameterValuesContainsInLookups;
                constructor(reportPreview: ReportPreview, parameterHelper?: PreviewParameterHelper);
                tabInfo: DevExpress.Analytics.Utils.TabInfo;
                popupInfo: any;
                parameterHelper: PreviewParameterHelper;
                initialize(originalParametersInfo: IReportParametersInfo): void;
                getPathsAfterPath(parameterPath: string): Array<string>;
                serializeParameters(): Array<IKeyValuePair<any>>;
                restore(): void;
                getInfo: ko.Observable<any>;
                isPropertyDisabled(name: string): boolean;
                getLookUpValues(changedParameterPath: string): void;
                submit: () => void;
                validateAndSubmit: (params: any) => void;
                needToRefreshLookUps: ko.Observable<boolean>;
                isEmpty: ko.Observable<boolean>;
                processInvisibleParameters: boolean;
                parametersLoading: ko.Observable<boolean>;
            }
            class PreviewParameterHelper extends ParameterHelper {
                callbacks?: Utils.IParametersCustomizationHandler;
                mapLookUpValues(type: string, lookUpValues: Array<ILookUpValue>): Array<Analytics.Utils.IDisplayedValue>;
                static fixPropertyName(propertyName: string): string;
                static getPrivatePropertyName(propertyName: string): string;
                createInfo(parameter: PreviewParameter): DevExpress.Analytics.Utils.ISerializationInfo;
                assignValueStore(info: DevExpress.Analytics.Utils.ISerializationInfo, parameter: PreviewParameter): void;
                isEnumType(parameter: PreviewParameter): boolean;
                getValueConverter(type: string): (val: any) => any;
                constructor(knownEnums?: Array<DevExpress.Reporting.IEnumType>, callbacks?: Utils.IParametersCustomizationHandler);
            }
        }
        module Internal {
            var formatSearchResult: (value: IFoundText) => string;
            interface IFoundText {
                pageIndex: number;
                indexes: string;
                id: number;
                text: string;
            }
            interface ISearchResult {
                matches: Array<IFoundText>;
                success: boolean;
            }
            interface ISearchEditorModel {
                findUp: () => void;
                findDown: () => void;
                loading: ko.Observable<boolean> | ko.Computed<boolean>;
                searchText: ko.Observable<string> | ko.Computed<string>;
                focusRequested: ko.Subscribable<boolean>;
            }
            class SearchViewModel extends Analytics.Utils.Disposable implements ISearchEditorModel, Analytics.Internal.IActionsProvider {
                private _cachedRequests;
                private _cachedWholeWordRequests;
                private _cachedCaseSensitiveRequests;
                private _cachedWholeWordWithCaseRequests;
                private _resultNavigator;
                static createResultNavigator: (seacrhModel: SearchViewModel, reportPreview: ReportPreview) => SearchResultNavigator;
                resetSearchResult(): void;
                findTextRequestDone(result: ISearchResult, cache: any): void;
                constructor(reportPreview: ReportPreview);
                getActions(context: any): Analytics.Utils.IAction[];
                tabInfo: Analytics.Utils.TabInfo;
                actions: Analytics.Utils.IAction[];
                findUp: () => void;
                findDown: () => void;
                goToResult: (result: IFoundText) => void;
                focusRequested: ko.Observable<boolean>;
                matchWholeWord: ko.Observable<boolean>;
                matchCase: ko.Observable<boolean>;
                searchUp: ko.Observable<boolean>;
                searchText: ko.Observable<string>;
                searchResult: ko.Observable<IFoundText[]>;
                readonly disabled: boolean;
                loading: ko.Observable<boolean>;
                findNext: () => void;
                clean: () => void;
            }
            interface ISearchResultNavigator {
                next: (up: boolean) => boolean;
                getFirstMatchFromPage: (pageIndex: number, up: boolean, thisPageOnly?: boolean) => IFoundText;
                currentResult: ko.Observable<IFoundText>;
                goToResult: (resultId: number) => void;
                searchResult: ko.Observable<IFoundText[]>;
            }
            class SearchResultNavigator extends Analytics.Utils.Disposable implements ISearchResultNavigator {
                constructor(searchModel: SearchViewModel, reportPreview: ReportPreview);
                next: (up: boolean) => boolean;
                getFirstMatchFromPage: (pageIndex: number, up: boolean, thisPageOnly?: boolean) => IFoundText;
                currentResult: ko.Observable<any>;
                goToResult: (resultId: number) => void;
                searchResult: ko.Observable<IFoundText[]>;
            }
            class dxSearchEditor extends dxTextBox {
                _$button: JQuery;
                _$buttonIcon: JQuery;
                _searchModel: any;
                _activeStateUnit: any;
                _focusRequestRaised: any;
                _koContext: any;
                constructor(element: any, options?: any);
                findNext(searchUp: boolean): boolean;
                _init(): void;
                _render(): void;
                _renderButton(direction: string): void;
                _attachButtonEvents(direction: string): void;
            }
            class DocumentMapItemsProvider implements DevExpress.Analytics.Utils.IItemsProvider {
                constructor(bookmark: IBookmarkNode);
                getItems: (IPathRequest: any) => JQueryPromise<IBookmarkDataMemberInfo[]>;
                private _selectNode;
                static fillNode(bookmark: IBookmarkNode): IBookmarkDataMemberInfo[];
                private _getRootNode;
                bookmarkDict: {};
            }
            class DocumentMapTreeListController implements DevExpress.Analytics.Widgets.Internal.ITreeListController {
                itemsFilter(item: DevExpress.Analytics.Utils.IDataMemberInfo): boolean;
                hasItems(item: DevExpress.Analytics.Utils.IDataMemberInfo): boolean;
                canSelect(value: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): boolean;
                select(value: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): void;
                showIconsForChildItems(): boolean;
                selectedItem: ko.Observable<Analytics.Widgets.Internal.TreeListItemViewModel>;
                clickHandler: (item: Analytics.Widgets.Internal.TreeListItemViewModel) => any;
            }
            interface IBookmarkNode {
                text: string;
                pageIndex: number;
                indexes: string;
                nodes?: Array<IBookmarkNode>;
            }
            interface IBookmarkDataMemberInfo extends DevExpress.Analytics.Utils.IDataMemberInfo {
                bookmark: IBookmarkNode;
            }
            class DocumentMapModel extends Analytics.Utils.Disposable {
                private _selectedPath;
                private _setSelectedPathByNavigationNode;
                constructor(reportPreview: ReportPreview);
                dispose(): void;
                tabInfo: Analytics.Utils.TabInfo;
                isEmpty: ko.Computed<boolean>;
                documentMapOptions: ko.Computed<Analytics.Widgets.Internal.ITreeListOptions>;
            }
        }
        module Widgets {
            module Internal {
                interface IMultiValueItem extends Analytics.Utils.IDisplayedValue {
                    selected?: ko.Observable<boolean> | ko.Computed<boolean>;
                    toggleSelected?: () => void;
                }
                class MultiValueEditorOptions extends Analytics.Utils.Disposable {
                    private _isValueSelected;
                    constructor(value: ko.Observable<any>, items: Array<Analytics.Utils.IDisplayedValue>);
                    selectedItems: ko.Observable<Array<IMultiValueItem>> | ko.Computed<Array<IMultiValueItem>>;
                    editorValue: ko.Observable<IMultiValueItem> | ko.Computed<IMultiValueItem>;
                    isSelectedAll: ko.Observable<boolean> | ko.Computed<boolean>;
                    _items: Array<IMultiValueItem>;
                    selectedValuesString: ko.Observable<string> | ko.Computed<string>;
                    displayItems: Array<IMultiValueItem>;
                    dataSource: any;
                    updateValue: () => void;
                    onOptionChanged: (e: any) => void;
                    value: ko.Observable | ko.Computed;
                }
            }
            enum PictureEditMode {
                Image = 0,
                Signature = 1,
                ImageAndSignature = 2
            }
            module Internal {
                class ImagePainter {
                    private _drawImage;
                    private _getImageSize;
                    private _getImageCoordinate;
                    constructor(options: {
                        imageSource: ko.Observable<string>;
                        sizeMode: ko.Observable<Editing.ImageSizeMode>;
                        alignment: ko.Observable<Editing.ImageAlignment>;
                    });
                    refresh(context: CanvasRenderingContext2D, scale?: number, contentSize?: any): JQueryPromise<{}>;
                    format: ko.Observable<string>;
                    image: ko.Observable<string> | ko.Computed<string>;
                    sizeMode: ko.Observable<Editing.ImageSizeMode>;
                    alignment: ko.Observable<Editing.ImageAlignment>;
                }
                class SignaturePainter extends DevExpress.Analytics.Utils.Disposable {
                    dispose(): void;
                    private _points;
                    private _lastX;
                    private _lastY;
                    private _drawPath;
                    private _drawCircle;
                    private _drawAllPoints;
                    drawCircle(context: any, x: any, y: any, color: any, width: any): void;
                    drawPath(context: any, x: any, y: any, color: any, width: any): void;
                    resetLastPosition(): void;
                    resetPoints(): void;
                    reset(): void;
                    refresh(context: any): void;
                    constructor();
                    hasPoints: ko.Computed<boolean>;
                }
                interface IPainterOptions {
                    imageSource: string;
                    imageType: string;
                    zoom: ko.Observable<number> | ko.Computed<number>;
                    sizeMode: Editing.ImageSizeMode;
                    alignment: Editing.ImageAlignment;
                    canDraw: ko.Observable<boolean> | ko.Computed<boolean>;
                }
                class Painter extends DevExpress.Analytics.Utils.Disposable {
                    private $element;
                    private _context;
                    private _getContextPoint;
                    private _pointerDownHandler;
                    private _pointerMoveHandler;
                    private _pointerLeaveHandler;
                    private _addEvents;
                    private _removeEvents;
                    private _setCanvasSize;
                    private _cleanCanvas;
                    constructor(options: IPainterOptions);
                    clear(): void;
                    refresh(): void;
                    initSize(element: JQuery, zoom: number): void;
                    initCanvas(element: JQuery, zoom: number): void;
                    getImage(): string;
                    hasSignature(): boolean;
                    dispose(): void;
                    reset(initialImage: any, initialAlignment: any, initialSizeMode: any): void;
                    initialSize: {
                        width: number;
                        height: number;
                    };
                    canDraw: boolean;
                    height: number;
                    format: (newVal?: string) => string;
                    image: ko.Observable<string> | ko.Computed<string>;
                    imageSizeMode: ko.Observable<Editing.ImageSizeMode>;
                    imageAlignment: ko.Observable<Editing.ImageAlignment>;
                    zoom: ko.Observable<number> | ko.Computed<number>;
                    scale: ko.Observable<number> | ko.Computed<number>;
                    lineWidth: ko.Observable<number>;
                    lineColor: ko.Observable<string>;
                    imagePainter: ImagePainter;
                    signaturePainter: SignaturePainter;
                }
                class PictureEditorToolbarItem implements IPictureEditorToolbarItem {
                    constructor(options: IPictureEditorToolbarItemOptions);
                    dispose(): void;
                    id: PictureEditorActionId;
                    icon: string;
                    title: string;
                    isActive: ko.Observable<boolean> | ko.Computed<boolean>;
                    renderedHandler: (element: HTMLElement, model: any) => void;
                    action: (e: any, model: any) => void;
                }
                class PictureEditorToolbarItemWithPopup extends PictureEditorToolbarItem implements IPictureEditorToolbarItemWithPopup {
                    constructor(options: IPictureEditorToolbarItemWithTemplateOptions<IPictureEditorActionPopupOptions>);
                    dispose(): void;
                    component: ko.Observable<IPopupComponent>;
                    template: string;
                    templateOptions: IPictureEditorActionPopup;
                }
                interface IPictureEditorToolbarItem extends IPictureEditorToolbarItemOptions {
                    dispose: () => void;
                }
                interface IPictureEditorToolbarItemWithPopup extends IPictureEditorToolbarItemWithTemplateOptions<IPictureEditorActionPopup> {
                    dispose: () => void;
                }
                interface IPictureEditorToolbarItemWithTemplateOptions<T> extends IPictureEditorToolbarItemOptions {
                    template: string;
                    templateOptions?: T;
                }
                interface IPictureEditorToolbarItemOptions {
                    id: PictureEditorActionId;
                    icon: string;
                    action?: (e: any, model: any) => void;
                    isActive: ko.Observable<boolean> | ko.Computed<boolean>;
                    renderedHandler?: (element: HTMLElement, model: any) => void;
                    title: string;
                }
                interface IPictureEditorActionPopup extends IPictureEditorActionPopupOptions {
                    component: IPopupComponent;
                    onContentReady: (e: {
                        element: any;
                        component: IPopupComponent;
                        model: IPictureEditorActionPopupOptions;
                    }) => void;
                    onShown: (e: {
                        element: any;
                        component: IPopupComponent;
                        model: IPictureEditorActionPopupOptions;
                    }) => void;
                    closeOnOutsideClick: (e: {
                        target: any;
                    }) => boolean;
                }
                interface IPopupComponent {
                    content: () => Element;
                    $element: () => JQuery;
                    dispose: () => void;
                }
                interface IPictureEditorActionPopupOptions {
                    width: string;
                    height: string;
                    contentTemplate: string;
                    contentData: any;
                    container: string;
                    target: string;
                    boundary: string | any;
                    getPositionTarget: () => any;
                    visible: ko.Observable<boolean> | ko.Computed<boolean>;
                }
                enum PictureEditorActionId {
                    OpenFile = 0,
                    PickImage = 1,
                    Alignment = 2,
                    Brush = 3,
                    Clear = 4,
                    Reset = 5
                }
                interface IImageEditorItem {
                    data?: string;
                    url?: string;
                    text?: string;
                    visible?: boolean | ko.Computed<boolean>;
                }
                class PictureEditorActionProvider extends Analytics.Utils.Disposable {
                    private _editorModel;
                    private _popupOptions;
                    static colors: string[];
                    private _getValues;
                    private _getColorValues;
                    private _initPopupOptions;
                    createOpenFileAction(action: (e: any) => void): PictureEditorToolbarItemWithPopup;
                    createImagePickerAction(images: IImageEditorItem[], filterEnabled: boolean, action: (base64: string) => void): PictureEditorToolbarItemWithPopup;
                    createSizingAction(): PictureEditorToolbarItemWithPopup;
                    createBrushAction(): PictureEditorToolbarItemWithPopup;
                    createResetItem(action: () => void): PictureEditorToolbarItem;
                    createClearItem(action: () => void): PictureEditorToolbarItem;
                    constructor(_editorModel: PictureEditorModel, _popupOptions: any);
                }
                class PictureEditorModel extends Analytics.Utils.Disposable {
                    private $element;
                    private _initialImage;
                    private _initialAlignment;
                    private _initialSizeMode;
                    private _pointerDownHandler;
                    private _pointerUpHandler;
                    private _pointerCancelHandler;
                    private _callbacks;
                    private GESTURE_COVER_CLASS;
                    private ACTIVE_POPUP_CLASS;
                    private _cacheLoadFileElement;
                    private _getPopupContent;
                    private _takeFocus;
                    private _releaseFocus;
                    private _wrapButtonAction;
                    private _initActions;
                    private _clearLoadedFile;
                    private _loadImageCallBack;
                    private _loadImage;
                    private _handleFiles;
                    private _addEvents;
                    constructor(options: IPictureEditorOptions, element: HTMLElement);
                    changeActiveButton(selectedItem: any): void;
                    applyBindings(): void;
                    dispose(): void;
                    getImage(): string;
                    reset(image: any, alignment: any, sizeMode: any): void;
                    getCurrentOptions(): IImageEditValue;
                    actionsProvider: PictureEditorActionProvider;
                    editMode: PictureEditMode;
                    actions: Array<IPictureEditorToolbarItem>;
                    painter: Painter;
                    isActive: ko.Observable<boolean> | ko.Computed<boolean>;
                    canDraw: ko.Observable<boolean> | ko.Computed<boolean>;
                    zoom: ko.Observable<number> | ko.Computed<number>;
                }
                interface IPictureEditorCallbacks {
                    onFocusOut: (s: any) => void;
                    onFocusIn?: (s: any) => void;
                    onDraw: (s: any) => void;
                    customizeActions?: (s: PictureEditorModel, actions: Array<IPictureEditorToolbarItem>) => void;
                }
                interface IImageEditValue {
                    sizeMode: Editing.ImageSizeMode;
                    alignment: Editing.ImageAlignment;
                    imageType: string;
                    image: string;
                }
                interface IPictureEditorOptions {
                    image: ko.Observable<string> | ko.Computed<string>;
                    imageType: ko.Observable<string> | ko.Computed<string>;
                    imageMode: ko.Observable<PictureEditMode> | ko.Computed<PictureEditMode>;
                    sizeMode: ko.Observable<Editing.ImageSizeMode> | ko.Computed<Editing.ImageSizeMode>;
                    alignment: ko.Observable<Editing.ImageAlignment> | ko.Computed<Editing.ImageAlignment>;
                    callbacks: IPictureEditorCallbacks;
                    isActive: ko.Observable<boolean> | ko.Computed<boolean>;
                    zoom: ko.Observable<number> | ko.Computed<number>;
                    popupOptions: IPictureEditorPopupTargetOptions;
                }
                interface IPictureEditorPopupTargetOptions {
                    target?: string;
                    container?: string;
                    boundary?: string;
                }
                interface IClickEvent {
                    target: HTMLElement;
                }
            }
            var editorTemplates: {
                multiValue: {
                    header: string;
                    extendedOptions: {
                        placeholder: () => any;
                        onMultiTagPreparing: (args: any) => void;
                    };
                };
                multiValueEditable: {
                    custom: string;
                };
                selectBox: {
                    header: string;
                };
            };
        }
        module Editing {
            class TextEditingFieldViewModelBase {
                keypressAction(data: any, event: any): void;
                hideEditor: (shouldCommit: boolean) => void;
            }
            class TextEditingFieldViewModel extends TextEditingFieldViewModelBase implements IEditingFieldViewModel {
                constructor(field: EditingField, pageWidth: number, pageHeight: number, zoom: ko.Observable<number> | ko.Computed<number>, bounds: IBounds);
                template: string;
                editorTemplate: string;
                field: EditingField;
                data: any;
                textStyle: () => {};
                containerStyle: () => {};
                breakOffsetStyle: () => {};
                borderStyle: () => {};
                zoom: ko.Observable<number> | ko.Computed<number>;
                htmlValue: () => string;
                wordWrap: boolean;
                active: ko.Observable<boolean>;
                activateEditor(viewModel: any, e: any): void;
            }
            enum GlyphStyle {
                StandardBox1 = 0,
                StandardBox2 = 1,
                YesNoBox = 2,
                YesNoSolidBox = 3,
                YesNo = 4,
                RadioButton = 5,
                Smiley = 6,
                Thumb = 7,
                Toggle = 8,
                Star = 9,
                Heart = 10
            }
            enum CheckState {
                Unchecked = 0,
                Checked = 1,
                Indeterminate = 2
            }
            function createCustomGlyphStyleCss(imageSource: DevExpress.Reporting.ImageSource): {};
            function getCheckBoxTemplate(style: string, state: string, customGlyph: {}): any;
            class CheckEditingFieldViewModel extends Analytics.Utils.Disposable implements IEditingFieldViewModel {
                private _editingFieldsProvider;
                private _toggleCheckState;
                constructor(field: EditingField, pageWidth: number, pageHeight: number, zoom: ko.Observable<number> | ko.Computed<number>, editingFieldsProvider: () => EditingField[]);
                template: string;
                field: EditingField;
                containerStyle: () => {};
                checkStyle: () => {};
                checkStateStyleIcon: any;
                customGlyphStyleCss: any;
                zoom: ko.Observable<number> | ko.Computed<number>;
                focused: ko.Observable<boolean>;
                onKeyDown(_: any, e: any): void;
                onBlur(): void;
                onFocus(): void;
                onClick(_: any, e: any): void;
                checked(): boolean;
                toggleCheckState(): void;
            }
            class ImageEditingFieldViewModel extends Analytics.Utils.Disposable implements IEditingFieldViewModel {
                field: EditingField;
                zoom: ko.Observable<number> | ko.Computed<number>;
                protected bounds: IBounds;
                protected popupTarget: string;
                constructor(field: EditingField, pageWidth: number, pageHeight: number, zoom: ko.Observable<number> | ko.Computed<number>, bounds: IBounds);
                getImage(): any;
                getImageType(): any;
                getPictureEditorOptions(): Widgets.Internal.IPictureEditorOptions;
                alignment: ko.Computed<ImageAlignment>;
                sizeMode: ko.Computed<ImageSizeMode>;
                editMode: Widgets.PictureEditMode;
                popupOptions: Widgets.Internal.IPictureEditorPopupTargetOptions;
                template: string;
                isActive: ko.Observable<boolean>;
                containerStyle: () => {};
                callbacks: Widgets.Internal.IPictureEditorCallbacks;
                onKeyDown(_: any, e: any): void;
                onFocusIn(s: Widgets.Internal.PictureEditorModel): void;
                onDraw(s: Widgets.Internal.PictureEditorModel): void;
                onBlur(s: Widgets.Internal.PictureEditorModel): void;
            }
            interface IImageEditingFieldPopupData {
                contentData: PopupImageEditingFieldViewModel;
                paintData: Viewer.Widgets.Internal.IPainterOptions;
                contentTemplate: string;
                isVisible: (element: HTMLElement) => boolean;
                getContainer: () => string;
                getPositionTarget: (element: HTMLElement) => JQuery;
                showContent: ko.Observable<boolean>;
                onShown: (e: {
                    element: any;
                    component: any;
                }) => void;
                onHiding: (e: {
                    element: any;
                    component: any;
                }) => void;
                onContentReady: (e: {
                    element: any;
                    component: Viewer.Widgets.Internal.IPopupComponent;
                }) => void;
                renderedHandler: (element: HTMLElement, model: any) => void;
            }
            class PopupImageEditingFieldViewModel extends Viewer.Editing.ImageEditingFieldViewModel implements Viewer.Editing.IEditingFieldViewModel {
                parentPopupClass: string;
                private _popupInitializedClass;
                private _getPopupContainer;
                private _getPainterModel;
                private _getPictureEditorModel;
                private _resetPictureEditor;
                private _resetPainter;
                isPopupActive(element: any): boolean;
                getPainter(): Widgets.Internal.IPainterOptions;
                getPopupData(): IImageEditingFieldPopupData;
                activateEditor(viewModel: any, e: any): void;
                popupData: IImageEditingFieldPopupData;
                painterData: Viewer.Widgets.Internal.IPainterOptions;
                template: string;
            }
            var DefaultImageEditingFieldViewModel: typeof PopupImageEditingFieldViewModel;
            class CharacterCombEditingFieldViewModel extends TextEditingFieldViewModelBase implements IEditingFieldViewModel {
                field: EditingField;
                constructor(field: EditingField, pageWidth: number, pageHeight: number, zoom: ko.Observable<number> | ko.Computed<number>, bounds: IBounds);
                cells: {
                    text: string;
                    style: any;
                }[];
                rowsCount: number;
                colsCount: number;
                template: string;
                containerStyle: () => {};
                textStyle: () => {};
                zoom: ko.Observable<number> | ko.Computed<number>;
                active: ko.Observable<boolean>;
                activateEditor(viewModel: any, e: any): void;
                static setText(cells: {
                    text: string;
                }[], textAlignment: string, rtl: boolean, text: string, rowsCount: number, colsCount: number): void;
            }
            interface IBounds {
                left: number;
                top: number;
                width: number;
                height: number;
                offset: {
                    x: number;
                    y: number;
                };
            }
            enum ImageAlignment {
                TopLeft = 1,
                TopCenter = 2,
                TopRight = 3,
                MiddleLeft = 4,
                MiddleCenter = 5,
                MiddleRight = 6,
                BottomLeft = 7,
                BottomCenter = 8,
                BottomRight = 9
            }
            enum ImageSizeMode {
                Normal = 0,
                StretchImage = 1,
                ZoomImage = 4,
                Squeeze = 5
            }
            interface IImageSourceBrickData {
                image: string;
                imageType: string;
            }
            interface IImageBrickData extends IImageSourceBrickData {
                alignment: ImageAlignment;
                sizeMode: ImageSizeMode;
            }
            interface IEditingFieldModel {
                id: string;
                groupID: string;
                readOnly: boolean;
                editorName: string;
                editValue: any | IImageBrickData;
                htmlValue: string;
                pageIndex: number;
                brickIndeces: string;
                type: string;
                bounds: IBounds;
                brickOptions: {
                    rtl: boolean;
                    rtlLayout: boolean;
                    formatString: string;
                    wordWrap: boolean;
                    style: string;
                    checkBoxBounds?: IBounds;
                    characterCombBounds?: IBounds[];
                    checkBoxGlyphOptions?: {
                        customGlyphs: {
                            key: number;
                            value: IImageSourceBrickData;
                        }[];
                        glyphStyle: Reporting.Viewer.Editing.GlyphStyle;
                    };
                };
            }
            interface IEditingFieldViewModel {
                template: string;
                field: EditingField;
                dispose?: () => void;
            }
            interface IEditingFieldHtmlProvider {
                getEditingFieldHtml: (value: string, editingFieldIndex: number) => JQueryPromise<string>;
            }
            class EditingField {
                protected _model: IEditingFieldModel;
                private _needToUseHtml;
                private _index;
                private _htmlProvider;
                constructor(model: IEditingFieldModel, index: number, htmlProvider: IEditingFieldHtmlProvider);
                private _refreshHtmlValue;
                editingFieldChanged(field: EditingField, oldVal: any, newVal: any): any;
                readOnly: ko.Observable<boolean> | ko.Computed<boolean>;
                modelValue: ko.Observable | ko.Computed;
                editValue: ko.Computed<any>;
                _editorValue: ko.Observable | ko.Computed;
                htmlValue: ko.Observable<string> | ko.Computed<string>;
                editorName(): string;
                id(): string;
                groupID(): string;
                pageIndex(): number;
                type(): string;
                model(): IEditingFieldModel;
                createViewModel(zoom: ko.Observable<number> | ko.Computed<number>, pageWidth: number, pageHeight: number, editingFieldsProvider: () => EditingField[], bounds: IBounds): IEditingFieldViewModel;
            }
        }
        module Export {
            module Metadata {
                var rtfExportModeMergedPreview: Analytics.Utils.ISerializationInfo;
                var docxExportModeMergedPreview: Analytics.Utils.ISerializationInfo;
                function excludeModesForMergedDocuments(val: string): ko.Observable<string>;
                function excludeDifferentFilesMode(val: string): ko.Observable<string>;
                var htmlExportModePreviewBase: Analytics.Utils.ISerializationInfo;
                var htmlExportModePreview: Analytics.Utils.ISerializationInfo;
                var htmlExportModeMergedPreview: Analytics.Utils.ISerializationInfo;
                var xlsExportModePreviewBase: Analytics.Utils.ISerializationInfo;
                var xlsExportModePreview: Analytics.Utils.ISerializationInfo;
                var xlsExportModeMergedPreview: Analytics.Utils.ISerializationInfo;
                var imageExportModePreviewBase: Analytics.Utils.ISerializationInfo;
                var imageExportModePreview: Analytics.Utils.ISerializationInfo;
                var imageExportModeMergedPreview: Analytics.Utils.ISerializationInfo;
                var xlsxExportModePreviewBase: Analytics.Utils.ISerializationInfo;
                var xlsxExportModePreview: Analytics.Utils.ISerializationInfo;
                var xlsxExportModeMergedPreview: Analytics.Utils.ISerializationInfo;
            }
            class CsvExportOptionsPreview extends Reporting.Export.CsvExportOptions {
                static from(model: any, serializer?: Analytics.Utils.IModelSerializer): CsvExportOptionsPreview;
                isPropertyVisible(name: string): boolean;
                isPropertyDisabled(name: string): boolean;
            }
            class DocxExportOptionsPreview extends Reporting.Export.DocxExportOptions {
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class DocxExportOptionsMergedPreview extends DocxExportOptionsPreview {
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                constructor(model: any, serializer: any);
            }
            class HtmlExportOptionsPreview extends Reporting.Export.HtmlExportOptions {
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class HtmlExportOptionsMergedPreview extends HtmlExportOptionsPreview {
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                constructor(model: any, serializer: any);
            }
            class ImageExportOptionsPreview extends Reporting.Export.ImageExportOptions {
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class ImageExportOptionsMergedPreview extends ImageExportOptionsPreview {
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                constructor(model: any, serializer: any);
            }
            class MhtExportOptionsPreview extends Reporting.Export.MhtExportOptions {
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class MhtExportOptionsMergedPreview extends MhtExportOptionsPreview {
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                constructor(model: any, serializer: any);
            }
            class RtfExportOptionsPreview extends Reporting.Export.RtfExportOptions {
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class RtfExportOptionsMergedPreview extends RtfExportOptionsPreview {
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                constructor(model: any, serializer: any);
            }
            class XlsExportOptionsPreview extends Reporting.Export.XlsExportOptions {
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class XlsExportOptionsMergedPreview extends XlsExportOptionsPreview {
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                constructor(model: any, serializer: any);
            }
            class XlsxExportOptionsPreview extends Reporting.Export.XlsxExportOptions {
                static toJson(value: any, serializer: any, refs: any): any;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class XlsxExportOptionsMergedPreview extends XlsxExportOptionsPreview {
                _getVariableInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                constructor(model: any, serializer: any);
            }
            class ExportOptionsPreview extends Reporting.Export.ExportOptions {
                _generateFromFunction(exportType: any): (model: any, serializer: any) => any;
                _generateInfo(): Analytics.Utils.ISerializationInfoArray;
                hasSensitiveData(): boolean;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class ExportOptionsMergedPreview extends ExportOptionsPreview {
                _generateInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class ExportOptionsModel extends Analytics.Utils.Disposable {
                private _reportPreview;
                constructor(reportPreview: ReportPreview);
                _getExportFormatItems(): Array<{
                    text: string;
                    format: string;
                }>;
                _exportDocumentByFormat(format: any): void;
                getActions(context: any): any[];
                dispose(): void;
                actions: any[];
                tabInfo: DevExpress.Analytics.Utils.TabInfo;
            }
        }
        class ReportPreview extends Analytics.Utils.Disposable {
            predefinedZoomLevels: ko.ObservableArray<number>;
            _pageWidth: ko.Observable<number>;
            _pageHeight: ko.Observable<number>;
            _pageBackColor: ko.Observable<string>;
            _currentReportId: ko.Observable<string>;
            _currentReportUrl: ko.Observable<string>;
            _currentDocumentId: ko.Observable<string>;
            _unifier: ko.Observable<string>;
            _currentOperationId: ko.Observable<string>;
            _stopBuildRequests: {
                [key: string]: boolean;
            };
            _closeDocumentRequests: {
                [key: string]: boolean;
            };
            _editingFields: ko.Observable<Editing.EditingField[]>;
            private _openReportOperationDeferred;
            _startBuildOperationId: string;
            private _editingValuesSubscriptions;
            private _drillDownState;
            private _sortingState;
            private _sortingProcessor;
            private _doDrillDown;
            private _doSorting;
            private _timeouts;
            private _deferreds;
            dispose(): void;
            removeEmptyPages(all?: boolean): void;
            private _initialize;
            createPage(pageIndex: number, processClick?: (target: Utils.IBrickNode) => void, loading?: ko.Observable<boolean>): Internal.PreviewPage;
            private _cleanTabInfo;
            private _clearReportInfo;
            private _window;
            private _initExportWindow;
            private _export;
            private _safelyRunWindowOpen;
            createBrickClickProcessor(cyclePageIndex: number): (brick: Utils.IBrickNode, e?: JQueryEventObject) => void;
            constructor(handlerUri?: string, previewRequestWrapper?: Internal.PreviewRequestWrapper, previewHandlersHelper?: Internal.PreviewHandlersHelper, callbacks?: Utils.IPreviewCustomizationHandler, rtl?: boolean);
            openReport(reportName: string): JQueryPromise<Utils.IPreviewInitialize>;
            drillThrough(customData?: string, closeCurrentReport?: boolean): JQueryPromise<Utils.IPreviewInitialize>;
            initialize(initializeDataPromise: JQueryPromise<Utils.IPreviewInitialize>): JQueryPromise<Utils.IPreviewInitialize>;
            private _deserializeExportOptions;
            deactivate(): void;
            startBuild(): JQueryPromise<boolean>;
            updateExportStatus(progress: number): void;
            customDocumentOperation(customData?: string, hideMessageFromUser?: boolean): JQueryPromise<Utils.IDocumentOperationResult>;
            _initializeStartBuild(): boolean;
            _startBuildRequest(): JQueryPromise<boolean>;
            getExportStatus(operationId: string): JQueryPromise<Internal.IExportProgressStatus>;
            getExportResult(operationId: string, inlineDisposition: boolean, token?: string, printable?: boolean, uri?: string): void;
            getBuildStatus(documentId: string): JQueryPromise<Internal.IDocumentBuildStatus>;
            getDoGetBuildStatusFunc(): (documentId: string) => void;
            getDocumentData(documentId: any): void;
            exportDocumentTo(format: string, inlineResult?: boolean): void;
            printDocument(pageIndex?: number): void;
            stopBuild(documentId?: string): void;
            closeDocument(documentId?: string): void;
            closeReport(reportId?: string): void;
            goToPage(pageIndex: number, forcePageChanging?: boolean, throttle?: number): void;
            private _goToPageTimer;
            getSelectedContent(): string;
            createEditingField(item: any, index: any): Editing.EditingField;
            rtlReport: ko.Observable<boolean>;
            rtlViewer: boolean;
            previewHandlersHelper: Internal.PreviewHandlersHelper;
            currentPage: ko.Observable<Internal.PreviewPage>;
            originalParametersInfo: ko.Observable<Parameters.IReportParametersInfo>;
            pageIndex: ko.Observable<number>;
            showMultipagePreview: ko.Observable<boolean>;
            documentMap: ko.Observable<Internal.IBookmarkNode>;
            exportOptionsModel: ko.Observable<Export.ExportOptionsPreview>;
            pageLoading: ko.Observable<boolean>;
            documentBuilding: ko.Observable<boolean>;
            progressBar: Internal.IProgressHandler;
            pages: ko.ObservableArray<Internal.PreviewPage>;
            customProcessBrickClick: (pageNamber: number, brick: Utils.IBrickNode, defaultHandler: () => void) => boolean;
            brickClickDocumentMapHandler: (navigationBrick: Utils.IBrickNodeNavigation) => void;
            editingFieldChanged: (field: Editing.EditingField, oldVal: any, newVal: any) => void;
            customizeExportOptions: (options: Utils.IPreviewExportOptionsCustomizationArgs) => void;
            isAutoFit: ko.Observable<boolean>;
            autoFitBy: ko.Observable<number>;
            exportDisabled: ko.PureComputed<boolean>;
            _zoom: ko.Observable<number>;
            zoom: ko.PureComputed<any>;
            editingFieldsProvider: () => Editing.EditingField[];
            _currentPageText: ko.PureComputed<any>;
            _getErrorMessage(jqXHR: any): any;
            _processError(error: string, jqXHR?: any, showForUser?: boolean): void;
            _raiseOnSizeChanged: () => void;
            previewSize: ko.Observable<number>;
            onSizeChanged: ko.Observable<any>;
            previewVisible: ko.Observable<boolean>;
            editingFieldsHighlighted: ko.Observable<boolean>;
            canSwitchToDesigner: boolean;
            allowURLsWithJSContent: boolean;
            requestWrapper: Internal.PreviewRequestWrapper;
            zoomStep: ko.Observable<number>;
            emptyDocumentCaption(): any;
            readonly reportId: string;
            readonly reportUrl: string;
            readonly documentId: string;
            exportOptionsTabVisible: ko.Observable<boolean>;
        }
        var MobilePreviewElements: {
            Surface: string;
            Search: string;
            Pages: string;
            MobileActions: string;
            Parameters: string;
        };
        module Mobile {
            module Internal {
                function updatePreviewContentSizeMobile(previewWrapperSize: ko.Observable<{
                    width: number;
                    height: number;
                }>, $root: JQuery): () => void;
                function updateMobilePreviewActionsPosition($actions: JQuery, $viewer: JQuery, $window: JQuery): (viewer: Element) => void;
                var slowdownDisctanceFactor: number;
                var minScale: number;
                class EventProcessor {
                    element: any;
                    slideOptions: ISlideOptions;
                    private _direction;
                    private _startingPositionX;
                    private _startingPositionY;
                    private _getFirstPageOffset;
                    getDirection(x?: any, y?: any): {
                        vertical: boolean;
                        horizontal: boolean;
                        scrollDown: boolean;
                    };
                    setPosition(x: any, y: any): void;
                    initialize(x: number, y: number): void;
                    start(e: JQueryEventObject): void;
                    move(e: JQueryEventObject): void;
                    end(e: JQueryEventObject): void;
                    constructor(element: any, slideOptions: ISlideOptions);
                    applySearchAnimation(value: any): void;
                    isLeftMove: boolean;
                    isRightMove: boolean;
                    latestY: number;
                    latestX: number;
                    $window: JQuery;
                    $element: JQuery;
                    $gallery: JQuery;
                    $galleryblocks: JQuery;
                    $body: JQuery;
                    firstMobilePageOffset: {
                        left: number;
                        top: number;
                    };
                }
                class MobilePaginator {
                    constructor(reportPreview: MobileReportPreview, gallery: GalleryModel);
                    visible: ko.Observable<boolean>;
                    text: ko.Observable<string> | ko.Computed<string>;
                }
                interface IMobileSearchPanel {
                    searchPanelVisible: ko.Observable<boolean> | ko.Computed<boolean>;
                    height: ko.Observable<number> | ko.Computed<number>;
                    editorVisible: ko.Observable<boolean> | ko.Computed<boolean>;
                }
                class MobileSearchViewModel extends Viewer.Internal.SearchViewModel implements IMobileSearchPanel {
                    static maxHeight: number;
                    focusEditor(event: any): void;
                    private _killSubscription;
                    private _updateBricks;
                    constructor(reportPreview: MobileReportPreview, gallery: GalleryModel);
                    updatePagesInBlocks(blocks: Array<IGalleryItemBlock>): void;
                    stopSearching(): void;
                    startSearch(): void;
                    editorVisible: ko.Observable<boolean> | ko.Computed<boolean>;
                    height: ko.Observable<number>;
                    searchPanelVisible: ko.Observable<boolean> | ko.Computed<boolean>;
                    enabled: ko.Observable<boolean> | ko.Computed<boolean>;
                }
                class SearchBarModel extends Analytics.Utils.Disposable {
                    private viewModel;
                    constructor(viewModel: MobileSearchViewModel, element: HTMLDivElement, $searchText: JQuery);
                    dispose(): void;
                }
                interface IGalleryItemBlock {
                    repaint?: boolean;
                    page: Viewer.Internal.PreviewPage;
                    classSet?: any;
                    visible?: boolean;
                    position: ko.Observable<IAbsolutePosition> | ko.Computed<IAbsolutePosition>;
                }
                interface IAbsolutePosition {
                    left: number;
                    top: number;
                    height: number;
                    width: number;
                }
                interface IGalleryItem {
                    blocks: ko.ObservableArray<IGalleryItemBlock>;
                    realIndex?: number;
                }
                class GalleryModel {
                    preview: MobileReportPreview;
                    private previewWrapperSize;
                    private _spacing;
                    private _animationTimeout;
                    private _createBlock;
                    constructor(preview: MobileReportPreview, previewWrapperSize: ko.Observable<{
                        width: number;
                        height: number;
                    }>);
                    updatePagesVisible(preview: MobileReportPreview): void;
                    updateCurrentBlock(): void;
                    updateContent(preview: MobileReportPreview, pagesCount: number): void;
                    updateBlockPositions(blocks: IGalleryItemBlock[], visible: any): void;
                    updateStartBlocks(galleryItem: IGalleryItem, pages: Viewer.Internal.PreviewPage[]): number;
                    updateLastBlocks(galleryItem: IGalleryItem, pages: Viewer.Internal.PreviewPage[]): number;
                    updateBlocks(galleryItem: IGalleryItem, pagesCount: number, preview: MobileReportPreview, index: any, useAnimation?: boolean): void;
                    changePage(preview: MobileReportPreview): void;
                    repaint: ko.Observable<{}>;
                    contentSize: ko.Observable<{
                        width: string;
                        height: string;
                    }> | ko.Computed<{
                        width: string;
                        height: string;
                    }>;
                    horizontal: ko.Observable<number> | ko.Computed<number>;
                    vertical: ko.Observable<number> | ko.Computed<number>;
                    pageCount: number;
                    isAnimated: ko.Observable<boolean> | ko.Computed<boolean>;
                    items: ko.ObservableArray<IGalleryItem>;
                    currentBlockText: ko.Observable<string> | ko.Computed<string>;
                    selectedIndexReal: ko.Observable<number>;
                    loopEnabled: ko.Computed<boolean>;
                    selectedIndex: ko.Observable<number>;
                    animationEnabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    swipeRightEnable: ko.Computed<boolean>;
                    swipeLeftEnable: ko.Computed<boolean>;
                }
                interface BlockItem {
                    element: JQuery;
                    left: number;
                }
                class dxGalleryReportPreview extends dxGallery {
                    private _animationClassName;
                    private blockItems;
                    private currentBlockItem;
                    private gallery;
                    private nextBlockItem;
                    private initializeBlockItems;
                    constructor(element: any, options?: any);
                    repaint(): void;
                    _swipeStartHandler(e: any): void;
                    _getNextIndex(offset: any): number;
                    _setSwipeAnimation(element: BlockItem, difference: any, offset: any, right: boolean): void;
                    _addAnimation(item: any): void;
                    _restoreDefault(item: BlockItem): void;
                    _getItem(index: any, loopTest: any): BlockItem;
                    _swipeUpdateHandler(e: any): void;
                    _swipeEndHandler(e: any): void;
                    _endSwipe(): void;
                }
                class MobilePreviewPage extends Viewer.Internal.PreviewPage {
                    constructor(preview: Viewer.Internal.IPreviewPageOwner, pageIndex: number, processClick?: (target: Viewer.Utils.IBrickNode) => void, loading?: ko.Observable<boolean>);
                    maxZoom: number;
                }
                interface IMobileActionContent {
                    name: string;
                    data: any;
                }
                interface IMobileAction {
                    imageClassName: string;
                    clickAction: () => void;
                    visible?: any;
                    content?: IMobileActionContent;
                }
                class MobileActionList {
                    actions: IMobileAction[];
                    constructor(actions: IMobileAction[]);
                    visible: ko.Observable<boolean>;
                }
                interface IPreviewActionsMobileOptions {
                    reportPreview: MobileReportPreview;
                    exportModel: Viewer.Export.ExportOptionsModel;
                    parametersModel: Viewer.Parameters.PreviewParametersViewModel;
                    searchModel: MobileSearchViewModel;
                    exportTypes: ko.ObservableArray<{
                        text: string;
                        format: string;
                    }>;
                    callbacks: Viewer.Utils.IPreviewCustomizationHandler;
                }
                function getPreviewActionsMobile(options: IPreviewActionsMobileOptions): MobileActionList;
                interface IDesignerModelPart {
                    id: string;
                    templateName: string;
                    model: any;
                }
                interface IMobileDesignerModel {
                    rootStyle: any;
                    reportPreview: MobileReportPreview;
                    parametersModel: Viewer.Parameters.PreviewParametersViewModel;
                    exportModel: Viewer.Export.ExportOptionsModel;
                    searchModel: MobileSearchViewModel;
                    rtl: boolean;
                    gallery?: GalleryModel;
                    paginator?: MobilePaginator;
                    brickEventsDisabled?: ko.Observable<boolean> | ko.Computed<boolean>;
                    slideOptions?: ISlideOptions;
                    parts?: Array<IDesignerModelPart>;
                    updateSurfaceSize?: () => void;
                    availableFormats: ko.ObservableArray<{
                        text: string;
                        format: string;
                    }>;
                }
                function createMobilePreview(element: Element, callbacks: Viewer.Utils.IPreviewCustomizationHandler, parametersInfo?: Viewer.Parameters.IReportParametersInfo, handlerUri?: string, previewVisible?: boolean, applyBindings?: boolean, allowURLsWithJSContent?: boolean, mobileModeSettings?: Viewer.Utils.IMobileModeSettings): IMobileDesignerModel;
            }
            interface ISlideOptions {
                disabled: ko.Observable<boolean> | ko.Computed<boolean>;
                readerMode: boolean;
                animationSettings: IPreviewAnimationSettings;
                searchPanel: Internal.IMobileSearchPanel;
                swipeEnabled: ko.Observable<boolean> | ko.Computed<boolean>;
                reachedTop: ko.Observable<boolean> | ko.Computed<boolean>;
                reachedLeft: ko.Observable<boolean> | ko.Computed<boolean>;
                reachedRight: ko.Observable<boolean> | ko.Computed<boolean>;
                scrollAvailable: ko.Observable<boolean> | ko.Computed<boolean>;
                zoomUpdating: ko.Observable<boolean> | ko.Computed<boolean>;
                galleryIsAnimated: ko.Observable<boolean> | ko.Computed<boolean>;
                autoFitBy: ko.Observable<number> | ko.Computed<number>;
                topOffset: ko.Observable<number> | ko.Computed<number>;
                brickEventsDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            interface IZoomOptions {
                zoomUpdating: ko.Observable<boolean> | ko.Computed<boolean>;
                zoom: ko.Observable<number> | ko.Computed<number>;
            }
            interface IPreviewAnimationSettings {
                zoomEnabled: ko.Observable<boolean> | ko.Computed<boolean>;
                swipeEnabled: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            class MobileReportPreview extends ReportPreview {
                constructor(handlerUri?: string, previewRequestWrapper?: Viewer.Internal.PreviewRequestWrapper, previewHandlersHelper?: Viewer.Internal.PreviewHandlersHelper, callbacks?: Viewer.Utils.IPreviewCustomizationHandler, rtl?: boolean, mobileSettings?: Viewer.Utils.IMobileModeSettings);
                createPage(pageIndex: number, processClick?: (target: Viewer.Utils.IBrickNode) => void, loading?: ko.Observable<boolean>): Internal.MobilePreviewPage;
                createBrickClickProcessor(cyclePageIndex: number): (brick: Utils.IBrickNode) => void;
                _hasActiveEditingFields(): boolean;
                showActions(s: MobileReportPreview): void;
                onSlide(e: any): void;
                availablePages: ko.Observable<number[]>;
                visiblePages: ko.Computed<Viewer.Internal.PreviewPage[]>;
                goToPage(pageIndex: any, forcePage?: any): void;
                getScrollViewOptions(): {
                    onUpdated: (e: any) => void;
                    direction: string;
                    pushBackValue: number;
                    bounceEnabled: boolean;
                };
                setScrollReached(e: any): void;
                readerMode: boolean;
                animationSettings: IPreviewAnimationSettings;
                topOffset: ko.Observable<number>;
                previewWrapperSize: ko.Observable<{
                    width: number;
                    height: number;
                }>;
                searchPanelVisible: ko.Observable<boolean>;
                interactionDisabled: ko.Observable<boolean> | ko.Computed<boolean>;
                actionsVisible: ko.Observable<boolean>;
                scrollReachedLeft: ko.Observable<boolean>;
                scrollReachedRight: ko.Observable<boolean>;
                scrollReachedTop: ko.Observable<boolean>;
                scrollReachedBottom: ko.Observable<boolean>;
                zoomUpdating: ko.Observable<boolean>;
                mobileZoom: ko.Computed<number>;
            }
            module Internal {
                class ParametersPopupModel extends Analytics.Utils.Disposable {
                    parametersModel: Parameters.PreviewParametersViewModel;
                    private _reportPreview;
                    private _parametersButtonContaner;
                    private _submit;
                    private _reset;
                    private _cancel;
                    constructor(parametersModel: Parameters.PreviewParametersViewModel, _reportPreview: MobileReportPreview);
                    initVisibilityIcons(): void;
                    cacheElementContent(element: any): void;
                    dispose(): void;
                    showIcons: ko.Observable<boolean>;
                    visible: ko.Observable<boolean> | ko.Computed<boolean>;
                    model: Parameters.PreviewParametersViewModel;
                    actionButtons: any;
                    actionIcons: any;
                    cancelDisabled: ko.Computed<boolean>;
                }
            }
        }
        class JSReportViewer {
            private _previewModel;
            previewModel: any;
            constructor(_previewModel: ko.Observable<any>);
            previewExists(): any;
            GetReportPreview(): any;
            GetPreviewModel(): any;
            GetParametersModel(): any;
            OpenReport(reportName: any): any;
            Print(pageIndex: any): any;
            ExportTo(format: any, inlineResult: any): void;
            GetCurrentPageIndex(): any;
            GoToPage(pageIndex: any): void;
            Close(): void;
            ResetParameters(): void;
            StartBuild(): any;
            UpdateLocalization(localization: any): void;
            AdjustControlCore(): void;
        }
        interface IReportViewerOptions {
            viewerModel?: any;
            reportPreview?: any;
            callbacks?: Utils.IPreviewCustomizationHandler;
            parts?: any[];
            requestOptions?: {
                host?: string;
                invokeAction: string;
                getLocalizationAction?: string;
            };
            documentId?: string;
            reportId?: string;
            reportUrl?: any;
        }
        class JSReportViewerBinding extends DevExpress.Analytics.Internal.JSDesignerBindingCommon<JSReportViewer> {
            private _callbacks;
            private _initializeEvents;
            private _initializeCallbacks;
            private _createModel;
            private _applyBindings;
            constructor(_options: IReportViewerOptions, customEventRaiser?: any);
            applyBindings(element: any): void;
        }
    }
}

/**
* DevExpress HTML/JS Reporting (dx-reportdesigner.d.ts)
* Version: 19.1.6
* Build date: 2019-09-10
* Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
* License: https://www.devexpress.com/Support/EULAs/NetComponents.xml
*/

declare module DevExpress.Reporting {
    module Chart {
        module Internal {
            class ChartRequests {
                static getChartImage(uri: string, chartLayout: any, width: number, height: number): any;
                static fieldListCallback(request: Analytics.Utils.IPathRequest): JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
            }
            class ChartStructureTreeListController extends Analytics.Internal.ObjectStructureTreeListController {
                constructor(propertyNames?: string[], listPropertyNames?: string[], selectCallback?: (value: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel) => void);
            }
            class ChartStructureObjectProvider extends Analytics.Internal.ObjectStructureProvider {
                getClassName(instance: any): any;
                constructor(target: any, displayName?: string, localizationId?: string);
            }
            function createInnerActionsWithPopover(text: any, id: any, actions: any): {
                text: any;
                imageClassName: string;
                imageTemplateName: string;
                disabled: ko.Observable<boolean>;
                id: any;
                _visible: ko.Observable<boolean>;
                popoverVisible: any;
                togglePopoverVisible: any;
                closePopover: any;
                templateName: string;
                getContainer: (element: HTMLElement, selector: string) => JQuery;
                actions: any;
            }[];
            function _isNumericTypeSpecific(specific: string): boolean;
            function _isDateTypeSpecific(specific: string): boolean;
            module Widgets {
                class ChartDataMemberEditor extends Analytics.Widgets.FieldListEditor {
                    private _isNumber;
                    private _isDate;
                    private _getArgumentDataMemberFilter;
                    private _getValueDataMemberFilter;
                    constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>);
                }
                class ChartDataSourceEditor extends Analytics.Widgets.Editor {
                    generateOptions(dataSources: ko.Computed<Array<{
                        displayName: string;
                        value: any;
                    }>>, popupContainer: string): any;
                    options: any;
                }
                class ChartDependencyEditor extends Analytics.Widgets.Editor {
                    constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                    getDependencyOptions(templateOptions: any, propertyName: any, depPropertyName: any): any;
                    depProperty: any;
                    bindableOptions: any;
                }
                class CollectionLookupEditorModel extends Analytics.Widgets.Editor {
                    constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                    readonly editors: any;
                    array: ko.Computed<any>;
                    selectedItem: ko.Observable<any>;
                }
                class PointsEditor extends Analytics.Widgets.Editor {
                    constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                    addPoint(model: any): Series.SeriesPointModel;
                }
                class SummaryFunctionModel {
                    static availableItems: string[];
                    static from(val: any): SummaryFunctionModel;
                    static toJson(value: SummaryFunctionModel): {};
                    private _updateArgs;
                    constructor(functionName: any, args: any);
                    functionName: ko.Observable<any>;
                    args: ko.ObservableArray<{
                        value: ko.Observable<string>;
                    }>;
                }
                class SummaryFunctionEditor extends Analytics.Widgets.FieldListEditor {
                    constructor(modelPropertyInfo: any, level: any, parentDisabled?: ko.Computed<boolean>);
                    getLocalization(displayName: any, localizationId: any): any;
                    memberPadding: any;
                    argumentTemplateName: string;
                    actionsAreAvailable: ko.Observable<boolean>;
                    add(): void;
                    remove(index: any): void;
                    availableItems(): string[];
                }
                class UndoColorPickerEditor extends Analytics.Widgets.ColorPickerEditor {
                    constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>);
                    generateValue(undoEngine: ko.Observable<Analytics.Utils.UndoEngine>): ko.Computed<any>;
                    generatedValue: ko.Computed<any>;
                }
                class ViewEditor extends Analytics.Widgets.Editor {
                    constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                    generateHeaderValue(undoEngine: ko.Observable<Analytics.Utils.UndoEngine>): ko.Computed<string>;
                    generateViewClassName(value: any, isTemplate?: boolean): any;
                    headerValue: ko.Computed<string>;
                    contentValue: ko.Computed<any>;
                }
            }
            var editorTemplates: {
                chartDataSource: {
                    header: string;
                    editorType: typeof Widgets.ChartDataSourceEditor;
                };
                collection: {
                    header: string;
                    content: string;
                    editorType: typeof Widgets.CollectionLookupEditorModel;
                };
                views: {
                    header: string;
                    content: string;
                    editorType: typeof Widgets.ViewEditor;
                };
                fieldChart: {
                    header: string;
                    editorType: typeof Analytics.Widgets.FieldListEditor;
                };
                dataMemberChart: {
                    header: string;
                    editorType: typeof Analytics.Widgets.DataMemberEditor;
                };
                valueDataMember: {
                    header: string;
                    editorType: typeof Widgets.ChartDataMemberEditor;
                };
                panes: {
                    header: string;
                };
                axisX: {
                    header: string;
                };
                axisY: {
                    header: string;
                };
                legends: {
                    header: string;
                };
                summaryFunction: {
                    header: string;
                    content: string;
                    editorType: typeof Widgets.SummaryFunctionEditor;
                };
                points: {
                    custom: string;
                    editorType: typeof Widgets.PointsEditor;
                };
                maxSize: {
                    header: string;
                    editorType: typeof Widgets.ChartDependencyEditor;
                };
                minSize: {
                    header: string;
                    editorType: typeof Widgets.ChartDependencyEditor;
                };
                group: {
                    header: string;
                };
                undoCustomColorEditor: {
                    header: string;
                    editorType: typeof Widgets.UndoColorPickerEditor;
                };
            };
            var defaultBooleanValues: Array<DevExpress.Analytics.Utils.IDisplayedValue>;
            var scaleTypeValues: Array<DevExpress.Analytics.Utils.IDisplayedValue>;
            var stringAlignmentValues: Array<DevExpress.Analytics.Utils.IDisplayedValue>;
            var paneName: Analytics.Utils.ISerializationInfo;
            var axisXName: Analytics.Utils.ISerializationInfo;
            var axisYName: Analytics.Utils.ISerializationInfo;
            var angle: Analytics.Utils.ISerializationInfo;
            var borderColor: Analytics.Utils.ISerializationInfo;
            var backColor: Analytics.Utils.ISerializationInfo;
            var dataMember: Analytics.Utils.ISerializationInfo;
            var text: Analytics.Utils.ISerializationInfo;
            var visible: Analytics.Utils.ISerializationInfo;
            var name: Analytics.Utils.ISerializationInfo;
            var tag: Analytics.Utils.ISerializationInfo;
            var checkedInLegend: Analytics.Utils.ISerializationInfo;
            var checkableInLegend: Analytics.Utils.ISerializationInfo;
            var legendText: Analytics.Utils.ISerializationInfo;
            var showInLegend: Analytics.Utils.ISerializationInfo;
            var thickness: Analytics.Utils.ISerializationInfo;
            var visibility: Analytics.Utils.ISerializationInfo;
            var color: Analytics.Utils.ISerializationInfo;
            var titleAlignment: Analytics.Utils.ISerializationInfo;
            var textPattern: Analytics.Utils.ISerializationInfo;
            var textAlignment: Analytics.Utils.ISerializationInfo;
            var maxLineCount: Analytics.Utils.ISerializationInfo;
            var maxWidth: Analytics.Utils.ISerializationInfo;
            var textColor: Analytics.Utils.ISerializationInfo;
            var antialiasing: Analytics.Utils.ISerializationInfo;
            var font: Analytics.Utils.ISerializationInfo;
            var enableAxisXZooming: Analytics.Utils.ISerializationInfo;
            var enableAxisXScrolling: Analytics.Utils.ISerializationInfo;
            var enableAxisYZooming: Analytics.Utils.ISerializationInfo;
            var enableAxisYScrolling: Analytics.Utils.ISerializationInfo;
            var rotated: Analytics.Utils.ISerializationInfo;
            var typeNameNotShow: Analytics.Utils.ISerializationInfo;
            var left: Analytics.Utils.ISerializationInfo;
            var right: Analytics.Utils.ISerializationInfo;
            var top: Analytics.Utils.ISerializationInfo;
            var bottom: Analytics.Utils.ISerializationInfo;
            var margin: Analytics.Utils.ISerializationInfo;
            var font18: Analytics.Utils.ISerializationInfo;
            var font12: Analytics.Utils.ISerializationInfo;
            var font8: Analytics.Utils.ISerializationInfo;
            var paneSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var defaultPane: Analytics.Utils.ISerializationInfo;
            var additionalPaneSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var chartDataSource: Analytics.Utils.ISerializationInfo;
            module Axis {
                class AxisXYViewModel extends Analytics.Elements.SerializableModel {
                    static from(info?: Analytics.Utils.ISerializationInfoArray): (model: any, serializer: any) => AxisXYViewModel;
                    static toJson(value: any, serializer: any, refs: any): any;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer, info?: Analytics.Utils.ISerializationInfoArray);
                    constantLines: ko.ObservableArray<Models.ConstantLineViewModel>;
                    scaleBreaks: ko.ObservableArray<Models.ScaleBreakViewModel>;
                    strips: ko.ObservableArray<Models.StripViewModel>;
                }
                class SecondaryAxisViewModel extends AxisXYViewModel implements ICollectionItem {
                    static xPrefix: string;
                    static yPrefix: string;
                    constructor(model: any, parent: ko.ObservableArray<SecondaryAxisViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                    readonly axisID: number;
                    parent: ko.ObservableArray<SecondaryAxisViewModel>;
                    innerActions: Array<Analytics.Utils.IAction>;
                }
                interface ICollectionItem {
                    parent: ko.ObservableArray<ICollectionItem>;
                    innerActions: Array<Analytics.Utils.IAction>;
                }
                function initCollectionItem(item: ICollectionItem, parent: ko.ObservableArray<ICollectionItem>): () => void;
            }
            module Models {
                class ChartElementCollectionItemBase extends Analytics.Elements.SerializableModel implements Axis.ICollectionItem {
                    static toJson(value: ChartElementCollectionItemBase, serializer: any, refs: any): any;
                    constructor(model: any, parent: ko.ObservableArray<ChartElementCollectionItemBase>, serializer?: Analytics.Utils.IModelSerializer, info?: Analytics.Utils.ISerializationInfoArray);
                    parent: ko.ObservableArray<Axis.ICollectionItem>;
                    name: ko.Observable<string> | ko.Computed<string>;
                    defaultItemName: (parentName?: string) => string;
                    innerActions: Analytics.Utils.IAction[];
                }
                class TitleViewModel extends ChartElementCollectionItemBase {
                    static prefix: string;
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): TitleViewModel;
                    constructor(model: any, parent: ko.ObservableArray<TitleViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                }
                class AdditionalLegendViewModel extends ChartElementCollectionItemBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): AdditionalLegendViewModel;
                    constructor(model: any, parent: ko.ObservableArray<AdditionalLegendViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                    static prefix: string;
                }
                class ConstantLineViewModel extends ChartElementCollectionItemBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): ConstantLineViewModel;
                    constructor(model: any, parent: ko.ObservableArray<ConstantLineViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                    static prefix: string;
                }
                class ScaleBreakViewModel extends ChartElementCollectionItemBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): ScaleBreakViewModel;
                    constructor(model: any, parent: ko.ObservableArray<ScaleBreakViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                    static prefix: string;
                }
                class StripViewModel extends ChartElementCollectionItemBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): StripViewModel;
                    constructor(model: any, parent: ko.ObservableArray<StripViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                    static prefix: string;
                }
                class AdditionalPaneViewModel extends ChartElementCollectionItemBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): AdditionalPaneViewModel;
                    constructor(model: any, parent: ko.ObservableArray<AdditionalPaneViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                    static prefix: string;
                }
                var dataFilterSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                class DataFilterModel implements Analytics.Utils.ISerializableModel {
                    static createNew(): DataFilterModel;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    columnName: ko.Observable<string>;
                    name: ko.Computed<string>;
                }
                var DefaultDataFilterModel: typeof DataFilterModel;
                class DataContainerViewModel extends Analytics.Elements.SerializableModel {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): DataContainerViewModel;
                    static toJson(value: any, serializer: any, refs: any): any;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    seriesTemplate: Series.SeriesTemplateViewModel;
                    series: ko.ObservableArray<Series.SeriesViewModel>;
                    dataMember: ko.Observable<string> | ko.Computed<string>;
                    seriesDataMember: ko.Observable<string> | ko.Computed<string>;
                    pivotGridDataSourceOptions: {
                        autoBindingSettingsEnabled: ko.Observable<boolean> | ko.Computed<boolean>;
                    };
                }
                class LegendViewModel extends Analytics.Elements.SerializableModel {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): LegendViewModel;
                    static toJson(value: any, serializer: any, refs: any): any;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                }
                class ChartViewModel extends Analytics.Elements.SerializableModel {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): ChartViewModel;
                    static toJson(value: any, serializer: any, refs: any): any;
                    _createDiagram(model: any, oldType: ko.Observable<string>, serializer: any): void;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    addTitle(model: any): void;
                    titles: ko.ObservableArray<TitleViewModel>;
                    legends: ko.ObservableArray<AdditionalLegendViewModel>;
                    dataContainer: DataContainerViewModel;
                    diagram: ko.Observable<DiagramViewModel> | ko.Computed<DiagramViewModel>;
                }
            }
            var typeNameSerializable: Analytics.Utils.ISerializationInfo;
            var barSeriesViewGroup: string[];
            var bar3DSeriesViewGroup: string[];
            var barWidth: Analytics.Utils.ISerializationInfo;
            var colorEach: Analytics.Utils.ISerializationInfo;
            var borderSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var border: Analytics.Utils.ISerializationInfo;
            var fillMode: Analytics.Utils.ISerializationInfo;
            var fillStyleOptionsSerialize: Analytics.Utils.ISerializationInfo;
            var fillMode3D: Analytics.Utils.ISerializationInfo;
            var fillStyle: Analytics.Utils.ISerializationInfo;
            var viewSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var fillModeMapper: {
                "Empty": any[];
                "Solid": Analytics.Utils.ISerializationInfo[];
                "Gradient": Analytics.Utils.ISerializationInfoArray;
                "Hatch": Analytics.Utils.ISerializationInfoArray;
            };
            module Series {
                class SummaryOptionsModelBase implements Analytics.Utils.ISerializableModel {
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    resetAllProperties(): void;
                    summaryFunction: DevExpress.Reporting.Chart.Internal.Widgets.SummaryFunctionModel;
                }
                class QualitativeSummaryOptionsModel extends SummaryOptionsModelBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): QualitativeSummaryOptionsModel;
                    static toJson(value: QualitativeSummaryOptionsModel, serializer: any, refs: any): any;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                }
                class NumericSummaryOptionsModel extends SummaryOptionsModelBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): NumericSummaryOptionsModel;
                    static toJson(value: QualitativeSummaryOptionsModel, serializer: any, refs: any): any;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                }
                class DateTimeSummaryOptionsModel extends SummaryOptionsModelBase {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): DateTimeSummaryOptionsModel;
                    static toJson(value: QualitativeSummaryOptionsModel, serializer: any, refs: any): any;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                }
                module Metadata {
                    var summaryFunctionSerializationInfo: Analytics.Utils.ISerializationInfo;
                    var summaryOptionsSerializationInfoArray: Analytics.Utils.ISerializationInfoArray;
                    var numericSummaryOptionsSerializationInfoArray: Analytics.Utils.ISerializationInfo[];
                    var dateTimeSummaryOptionsSerializationInfoArray: Analytics.Utils.ISerializationInfo[];
                }
                class FillStyle extends Analytics.Elements.SerializableModel {
                    static from(info: any, gradientTypeName: any): (model: any, serializer: any) => FillStyle;
                    static toJson(model: FillStyle, serializer: Analytics.Utils.IModelSerializer, refs: any): any;
                    private _optionsTypeMap;
                    constructor(model: any, info: Analytics.Utils.ISerializationInfoArray, gradientTypeName: string, serializer?: Analytics.Utils.IModelSerializer);
                    isPropertyVisible(propertyName: any): any;
                    updateOptions(fillMode: string, serializer: any, optionsObject: any): void;
                    fillMode: ko.Observable<string> | ko.Computed<string>;
                    options: ko.Observable<any>;
                    gradientTypeName: string;
                }
            }
            var viewMapper: {
                FullStackedStepAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                PolarRangeAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                RadarRangeAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                RangeArea3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                RangeAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedStepAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                StepArea3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                StepAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideFullStackedBar3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideFullStackedBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideStackedBar3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideStackedBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedLine3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                ScatterPolarLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                ScatterRadarLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedLine3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                NestedDoughnutSeriesView: Analytics.Utils.ISerializationInfoArray;
                SwiftPlotSeriesView: Analytics.Utils.ISerializationInfoArray;
                Funnel3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                FunnelSeriesView: Analytics.Utils.ISerializationInfoArray;
                ScatterLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                BubbleSeriesView: Analytics.Utils.ISerializationInfoArray;
                Spline3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                SplineArea3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedSplineArea3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                SplineAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedSplineAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedSplineArea3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                SplineSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedSplineAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                Area3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedArea3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                PolarAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                RadarAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedArea3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedBar3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideBar3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedBar3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                PolarLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                RadarLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                Doughnut3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                DoughnutSeriesView: Analytics.Utils.ISerializationInfoArray;
                PolarPointSeriesView: Analytics.Utils.ISerializationInfoArray;
                OverlappedGanttSeriesView: Analytics.Utils.ISerializationInfoArray;
                RadarPointSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideGanttSeriesView: Analytics.Utils.ISerializationInfoArray;
                AreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                CandleStickSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                FullStackedBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                Line3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                LineSeriesView: Analytics.Utils.ISerializationInfoArray;
                ManhattanBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                OverlappedRangeBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                Pie3DSeriesView: Analytics.Utils.ISerializationInfoArray;
                PieSeriesView: Analytics.Utils.ISerializationInfoArray;
                PointSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                SideBySideRangeBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedAreaSeriesView: Analytics.Utils.ISerializationInfoArray;
                StackedBarSeriesView: Analytics.Utils.ISerializationInfoArray;
                StepLineSeriesView: Analytics.Utils.ISerializationInfoArray;
                StockSeriesView: Analytics.Utils.ISerializationInfoArray;
                StepLine3DSeriesView: Analytics.Utils.ISerializationInfoArray;
            };
            module DataMembers {
                class DataMemberBase extends Analytics.Utils.Disposable {
                    private _separator;
                    private _assignValueDataMembers;
                    private _valueDataMembersToString;
                    toString(): string;
                    constructor(value: any, valueScaleType?: any);
                    valueScaleType: any;
                    arrayValueDataMemberNames: string[];
                }
                class CommonValueDataMembers extends DataMemberBase {
                    static from(value: any): CommonValueDataMembers;
                    static toJson(value: any): any;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    readonly arrayValueDataMemberNames: string[];
                }
                class StockValueDataMembers extends DataMemberBase {
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    readonly arrayValueDataMemberNames: string[];
                }
                class Value1Value2DataMembers extends DataMemberBase {
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    readonly arrayValueDataMemberNames: string[];
                }
                class ValueWeightDataMembers extends DataMemberBase {
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    readonly arrayValueDataMemberNames: string[];
                }
            }
            module Series {
                interface IViewBindableProperty {
                    model: ko.Observable<any>;
                    type: ko.Observable<string> | ko.Computed<string>;
                }
                enum ScaleType {
                    Qualitative = 0,
                    Numerical = 1,
                    DateTime = 2,
                    Auto = 3
                }
                var ScaleTypeMap: {
                    [key: string]: ScaleType;
                };
                class SeriesTemplateViewModel extends Analytics.Elements.SerializableModel {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): SeriesTemplateViewModel;
                    static toJson(value: SeriesTemplateViewModel, serializer: any, refs: any): any;
                    updateByView(view: SeriesViewViewModel): void;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer, info?: Analytics.Utils.ISerializationInfoArray);
                    isPropertyVisible(propertyName: any): boolean;
                    viewBindable: IViewBindableProperty;
                    viewType: ko.Observable<string> | ko.Computed<string>;
                    view: ko.Observable<SeriesViewViewModel>;
                    label: SeriesLabelViewModel;
                    argumentDataMember: ko.Observable<string> | ko.Computed<string>;
                    argumentScaleType: ko.Observable<string> | ko.Computed<string>;
                    valueScaleType: ko.Observable<string> | ko.Computed<string>;
                    valueDataMembers: any;
                    dataFilters: ko.ObservableArray<Internal.Models.DataFilterModel>;
                    qualitativeSummaryOptions: Internal.Series.QualitativeSummaryOptionsModel;
                    numericSummaryOptions: Internal.Series.NumericSummaryOptionsModel;
                    dateTimeSummaryOptions: Internal.Series.DateTimeSummaryOptionsModel;
                    _actualArgumentScaleType: ko.Observable<ScaleType>;
                }
                class SeriesLabelViewModel extends Analytics.Elements.SerializableModel {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): SeriesLabelViewModel;
                    static toJson(value: any, serializer: any, refs: any): any;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    typeNameSerializable: ko.Observable<string> | ko.Computed<string>;
                }
                class SeriesPointModel extends Analytics.Elements.SerializableModel {
                    static separator: string;
                    static getSerializationValue(array: Array<ko.Observable<any>>, dateConverter: any): any[];
                    static createNew(series: any): SeriesPointModel;
                    static valueToJsonObject(value: any): string;
                    private _valueDataMembersToString;
                    private _assignValueDataMembers;
                    constructor(model: any, series: SeriesViewModel, serializer?: Analytics.Utils.IModelSerializer);
                    readonly isDateType: boolean;
                    valuesSerializable: ko.Observable | ko.Computed;
                    series: SeriesViewModel;
                    arrayValueDataMemberNames: string[];
                }
                class SeriesViewViewModel extends Analytics.Elements.SerializableModel {
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): ko.Observable<SeriesViewViewModel>;
                    dispose(): void;
                    static toJson(value: any, serializer: any, refs: any): any;
                    _getInfo(typeName: string): Analytics.Utils.ISerializationInfo[];
                    private _createPropertyDisabledDependence;
                    private _createMarkerDependences;
                    private _createLinkOptionsDependences;
                    preInitProperties(model: any): void;
                    constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                    axisXName: ko.Observable<string> | ko.Computed<string>;
                    axisYName: ko.Observable<string> | ko.Computed<string>;
                    paneName: ko.Observable<string> | ko.Computed<string>;
                    fillStyle: FillStyle;
                    barWidth: ko.Observable<number> | ko.Computed<number>;
                    typeName: string;
                }
                class SeriesViewModel extends SeriesTemplateViewModel implements Axis.ICollectionItem {
                    static prefix: string;
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): SeriesViewModel;
                    static toJson(value: any, serializer: any, refs: any): any;
                    static getClassName(typeName: any): any;
                    updateByView(view: SeriesViewViewModel): void;
                    constructor(model: any, parent: ko.ObservableArray<SeriesViewModel>, serializer?: Analytics.Utils.IModelSerializer);
                    isIncompatible: ko.Observable<boolean>;
                    parent: ko.ObservableArray<SeriesViewModel>;
                    points: ko.ObservableArray<SeriesPointModel>;
                    innerActions: Analytics.Utils.IAction[];
                }
            }
            interface IDiagramViewModel {
                axisX?: any;
                axisY?: any;
                secondaryAxesX?: ko.ObservableArray<Axis.SecondaryAxisViewModel>;
                secondaryAxesY?: ko.ObservableArray<Axis.SecondaryAxisViewModel>;
                defaultPanes?: any;
                panes?: ko.ObservableArray<Models.AdditionalPaneViewModel>;
                getInfo: () => Analytics.Utils.ISerializationInfoArray;
            }
            class DiagramViewModel extends Analytics.Elements.SerializableModel implements IDiagramViewModel {
                static createDiagram(model: any, type: any, serializer?: Analytics.Utils.ModelSerializer): IDiagramViewModel;
                static from(model: any, serializer?: Analytics.Utils.IModelSerializer): DiagramViewModel;
                static toJson(value: any, serializer: any, refs: any): any;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                secondaryAxesX: ko.ObservableArray<Axis.SecondaryAxisViewModel>;
                secondaryAxesY: ko.ObservableArray<Axis.SecondaryAxisViewModel>;
                panes: ko.ObservableArray<Models.AdditionalPaneViewModel>;
            }
            var scaleBreakSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var legendName: Analytics.Utils.ISerializationInfo;
            var stripSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var constantLineSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var axisXYSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var secondaryAxisXYSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var radarAxisX: Analytics.Utils.ISerializationInfo;
            var radarAxisY: Analytics.Utils.ISerializationInfo;
            var axisX3D: Analytics.Utils.ISerializationInfo;
            var axisY3D: Analytics.Utils.ISerializationInfo;
            var axisX: Analytics.Utils.ISerializationInfo;
            var axisY: Analytics.Utils.ISerializationInfo;
            var diagram: Analytics.Utils.ISerializationInfo;
            var secondaryAxesX: Analytics.Utils.ISerializationInfo;
            var secondaryAxesY: Analytics.Utils.ISerializationInfo;
            var panes: Analytics.Utils.ISerializationInfo;
            var diagramSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var diagramMapper: {
                [key: string]: {
                    info: Analytics.Utils.ISerializationInfoArray;
                    type: string;
                };
            };
            var padding: Analytics.Utils.ISerializationInfo;
            var seriesLabelSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var seriesLabel: Analytics.Utils.ISerializationInfo;
            var pivotGridDataSourceOptions: Analytics.Utils.ISerializationInfo;
            var seriesPointSerializationsInfo: Analytics.Utils.ISerializationInfo[];
            var points: Analytics.Utils.ISerializationInfo;
            var createViewsArray: (limitation: any) => any[];
            var view: Analytics.Utils.ISerializationInfo;
            var viewBindableSerializationInfo: Analytics.Utils.ISerializationInfo;
            var qualitativeSummaryOptions: Analytics.Utils.ISerializationInfo;
            var numericSummaryOptions: Analytics.Utils.ISerializationInfo;
            var dateTimeSumaryOptions: Analytics.Utils.ISerializationInfo;
            var seriesTemplateSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var seriesTemplate: Analytics.Utils.ISerializationInfo;
            var seriesSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var seriesSerializable: Analytics.Utils.ISerializationInfo;
            var seriesDataMember: Analytics.Utils.ISerializationInfo;
            var dataContainerSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var dataContainer: Analytics.Utils.ISerializationInfo;
            var titleSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var titles: Analytics.Utils.ISerializationInfo;
            var legendSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var legend: Analytics.Utils.ISerializationInfo;
            var additionalLegendSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var legends: Analytics.Utils.ISerializationInfo;
            var appearanceName: Analytics.Utils.ISerializationInfo;
            var paletteName: Analytics.Utils.ISerializationInfo;
            var chartSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            var chart: Analytics.Utils.ISerializationInfo;
            var chartDataMember: Analytics.Utils.ISerializationInfo;
            var chartSeriesDataMember: Analytics.Utils.ISerializationInfo;
            var fakeChartSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            var chartControlSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            interface IChartControlOptions {
                chartSource?: any;
                chart?: Models.ChartViewModel;
                dataSource: ko.Observable<Analytics.Internal.IDataSourceInfo> | ko.Computed<Analytics.Internal.IDataSourceInfo>;
                size?: Analytics.Elements.ISize;
                disabled?: ko.Observable<boolean> | ko.Computed<boolean>;
                callbacks?: IChartControlCallbacks;
            }
            class ChartControlViewModel extends Analytics.Utils.Disposable {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                getControlFactory(): Analytics.Utils.ControlsFactory;
                isSeriesPropertyDisabled(name: any): boolean;
                isSeriesTemplatePropertyDisabled(name: any): boolean;
                private _getSeriesActualArgumentScaleType;
                private _initSeries;
                private _initChartElementFunctions;
                constructor(options: IChartControlOptions);
                getPath(propertyName: string): string;
                serialize(): any;
                save(): any;
                isPropertyDisabled(name: string): boolean;
                chart: Models.ChartViewModel;
                onSave: (data: any) => void;
                dataSource: ko.Observable<Analytics.Internal.IDataSourceInfo> | ko.Computed<Analytics.Internal.IDataSourceInfo>;
                seriesDataMember: ko.Observable<string> | ko.Computed<string>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                fieldListProvider: ko.Observable<Analytics.Internal.FieldListProvider>;
            }
            class ChartControlSurface extends Analytics.Utils.Disposable {
                constructor(control: ChartControlViewModel, zoom?: ko.Observable<number>, size?: Analytics.Elements.ISize);
                width: ko.Computed<number>;
                height: ko.Computed<number>;
                imageSrc: ko.Observable<string>;
                zoom: ko.Observable<number> | ko.Computed<number>;
                templateName: string;
            }
            var ActionId: {
                Save: string;
            };
            var controlsFactory: Analytics.Utils.ControlsFactory;
            function registerControls(): void;
            function deserializeModelArray<T>(model: any, creator: (item: any, parent: any) => T, prefix: string): ko.ObservableArray<T>;
            function parseDate(val: any): Date;
            function serializeDate(date: Date): string;
            var HandlerUri: string;
            function updateChartSurfaceContentSize(element: any, surfaceSize: ko.Observable<number> | ko.Computed<number>, rtl?: boolean): () => void;
            interface IChartControlCallbacks {
                fieldLists?: (IPathRequest: any) => JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
                customizeActions?: (actions: Analytics.Utils.IAction[]) => void;
                init?: (designerModel: any) => void;
            }
            interface IChartDesignerOptions {
                data: {
                    chartSource?: ko.Observable<any>;
                    chart?: ko.Observable<ChartControlViewModel>;
                    dataSource?: ko.Observable<Analytics.Internal.IDataSourceInfo>;
                    availableChartDataSources?: ko.Computed<Array<{
                        displayName: string;
                        value: any;
                    }>>;
                    width?: number;
                    height?: number;
                };
                fieldListProvider?: Analytics.Internal.FieldListProvider;
                callbacks?: IChartControlCallbacks;
                localization?: any;
                rtl?: boolean;
            }
            function subscribeTreelistArray(chartStructureProvider: any, array: ko.ObservableArray<any>, getPath: () => string[], subscribeNewItem?: (item: any, array: any, path: any) => void): ko.Subscription;
            function getPropertyInfo(serializationsInfo: Analytics.Utils.ISerializationInfoArray, index: number, pathComponets: any): Analytics.Utils.ISerializationInfo;
            function createChartStructure(chart: ChartControlViewModel, selectedItem: any, subscriptions: any): {
                itemsProvider: ChartStructureObjectProvider;
                treeListController: ChartStructureTreeListController;
                expandRootItems: boolean;
                selectedPath: ko.Observable<string> | ko.Computed<string>;
            };
            function createChartDesigner(element: Element, options: IChartDesignerOptions, applyBindings?: boolean): any;
        }
    }
    module Designer {
        module Actions {
            var ActionId: {
                NewReport: string;
                NewReportViaWizard: string;
                OpenReport: string;
                ReportWizard: string;
                ReportWizardFullScreen: string;
                Preview: string;
                Scripts: string;
                AddDataSource: string;
                AddSqlDataSource: string;
                AddMultiQuerySqlDataSource: string;
                ValidateBindings: string;
                Save: string;
                SaveAs: string;
                Exit: string;
                FullScreen: string;
            };
        }
        module Internal {
            class AlignmentHandler {
                private _selectionProvider;
                private _surfaceContext;
                constructor(selectionProvider: Analytics.Internal.ISelectionProvider, surfaceContext: ko.Observable<Analytics.Elements.ISurfaceContext>);
                private _getFocusedItem;
                private _getFocusedParent;
                private _getPositionFromBand;
                private _visitAllSelectedItemsInSameContainerWithFocused;
                private _centerByBand;
                private _roundingValue;
                alignLeft(): void;
                alignTop(): void;
                alignRight(): void;
                alignBottom(): void;
                alignVerticalCenters(): void;
                alignHorizontalCenters(): void;
                sizeToControlWidth(): void;
                sizeToControlHeight(): void;
                sizeToControl(): void;
                centerHorizontally(): void;
                centerVertically(): void;
                alignToGrid(): void;
                sizeToGrid(): void;
                sendToBack(): void;
                bringToFront(): void;
                canChangeZOrder(): boolean;
            }
        }
        module Actions {
            class ElementActions extends Analytics.Internal.BaseActionsProvider {
                private _selectionProvider;
                private _generalDisabled;
                private _isMultiSelect;
                constructor(surfaceContext: ko.Observable<Analytics.Elements.ISurfaceContext>, selectionProvider: Analytics.Internal.ISelectionProvider);
                condition(context: any): boolean;
            }
            class ElementsGroupActions extends Analytics.Internal.BaseActionsProvider {
                private _selectionProvider;
                actions: Analytics.Utils.IAction[];
                constructor(surfaceContext: ko.Observable<Analytics.Elements.ISurfaceContext>, selectionProvider: Analytics.Internal.ISelectionProvider);
                condition(context: any): boolean;
            }
            class FitBoundsToTextAction {
                _control: Controls.XRTextControlSurfaceBase<Analytics.Elements.ElementViewModel>;
                textElementHelper: Internal.TextElementSizeHelper;
                private _getNewRectForVetical;
                private _findWidth;
                private _getNewRectForHorizontal;
                private _getTextContainerSize;
                private _getTextHeight;
                fitWidth(): void;
                fitHeight(): void;
                fitBounds(): void;
                constructor(_control: Controls.XRTextControlSurfaceBase<Analytics.Elements.ElementViewModel>, textElementHelper?: Internal.TextElementSizeHelper);
            }
            class FitTextToBoundsAction {
                _control: Controls.XRTextControlSurfaceBase<Analytics.Elements.ElementViewModel>;
                textElementHelper: Internal.TextElementSizeHelper;
                private _getTextSide;
                private _calculateFont;
                private _getAvailableFont;
                fit(): void;
                constructor(_control: Controls.XRTextControlSurfaceBase<Analytics.Elements.ElementViewModel>, textElementHelper?: Internal.TextElementSizeHelper);
            }
            class FitToContainerAction {
                private _control;
                private _container;
                constructor(_control: ko.Observable<Analytics.Elements.SurfaceElementBase<Analytics.Elements.ElementViewModel>>);
                doAction(): void;
                allowed(): boolean;
                visible(): boolean;
            }
            class PivotGridActions extends Analytics.Internal.BaseActionsProvider {
                constructor();
                condition(context: any): boolean;
            }
            class ReportActions implements Analytics.Internal.IActionsProvider {
                actions: Analytics.Utils.IAction[];
                private _contextModel;
                private _targetModel;
                private _canAddBand;
                private _addBand;
                constructor(onComponentAdded?: any);
                getActions(context: any): Analytics.Utils.IAction[];
                onComponentAdded: (e: Utils.IComponentAddedEventArgs) => void;
            }
            class ReportElementActions extends ElementActions {
                constructor(surfaceContext: ko.Observable<Analytics.Elements.ISurfaceContext>, selection: Analytics.Internal.ISelectionProvider);
                getActions(context: any): Analytics.Utils.IAction[];
            }
        }
        module Internal {
            class SpaceCommandHandler {
                private _selectionProvider;
                private _surfaceContext;
                constructor(selectionProvider: Analytics.Internal.ISelectionProvider, surfaceContext: ko.Observable<Analytics.Elements.ISurfaceContext>);
                private _comparer;
                private _spaceIncrease;
                private _spaceMakeEqual;
                private _concatenateWithSpace;
                horizSpaceConcatenate(): void;
                vertSpaceConcatenate(): void;
                horizSpaceMakeEqual(): void;
                vertSpaceMakeEqual(): void;
                horizSpaceDecrease(): void;
                horizSpaceIncrease(): void;
                vertSpaceDecrease(): void;
                vertSpaceIncrease(): void;
            }
            class DataSourceActions implements Analytics.Internal.IActionsProvider {
                private _dsHelper;
                private _reportViewModel;
                private _undoEngine;
                private _findDataSource;
                constructor(dsHelper: ko.Observable<DataSourceHelper> | ko.Computed<DataSourceHelper>, reportViewModel: ko.Observable<Controls.ReportViewModel> | ko.Computed<Controls.ReportViewModel>, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>);
                removeDataSource(dataSourceID: string): void;
                removeDataSourceAction: {
                    clickAction: (item: any) => void;
                    position: number;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: any;
                };
                getActions(context: any): Analytics.Utils.IAction[];
            }
            interface IDataSourceInfo extends Analytics.Internal.IDataSourceInfo {
                base64: () => string;
            }
            class CreateQueryIterator extends Analytics.Wizard.DataSourceWizardPageIterator {
                getNextPageId(pageId: string): any;
            }
            class SqlDataSourceEditor implements Analytics.Internal.IActionsProvider {
                private _dsHelper;
                private _wizard;
                private _reportViewModel;
                private _undoEngine;
                private _itemsProvider;
                private _applyWizardChanges;
                private _createOrEditSqlDataSource;
                private _applyDataSourceChange;
                private _findDataSource;
                private static _onFail;
                constructor(dsHelper: ko.Observable<DataSourceHelper> | ko.Computed<DataSourceHelper>, dataSourceWizard: Analytics.Wizard.DataSourceWizard, reportViewModel: ko.Observable<Controls.ReportViewModel> | ko.Computed<Controls.ReportViewModel>, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, itemsProvider: ko.Observable<Analytics.Utils.IItemsProvider> | ko.Computed<Analytics.Utils.IItemsProvider>);
                relationsEditor: ko.Observable<QueryBuilder.Widgets.Internal.MasterDetailEditor>;
                editSqlQuery(dataSourceID: string, queryName: string): void;
                addSqlQuery(dataSourceID: string): void;
                removeSqlQuery(dataSourceID: string, queryName: string): void;
                editMasterDetailRelations(dataSourceID: string): void;
                applySqlDataSourceWizardChanges(dataSourceWizardModel: Analytics.Wizard.IDataSourceWizardState): JQueryPromise<IDataSourceInfo>;
                static createSqlDataSourceInfo(source: Analytics.Data.SqlDataSource, queryName?: string, relationsEditing?: boolean): JQueryPromise<IDataSourceInfo>;
                addAction: {
                    clickAction: (item: any) => void;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: any;
                };
                editAction: {
                    clickAction: (item: any) => void;
                    position: number;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: any;
                };
                removeAction: {
                    clickAction: (item: any) => void;
                    position: number;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: any;
                };
                editRelationsAction: {
                    clickAction: (item: any) => void;
                    position: number;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: any;
                };
                getActions(context: any): Analytics.Utils.IAction[];
            }
            class EditSchemaIterator extends Analytics.Wizard.DataSourceWizardPageIterator {
                getNextPageId(pageId: any): any;
            }
            class JsonDataSourceEditor implements Analytics.Internal.IActionsProvider {
                private _dsHelper;
                private _wizard;
                private _reportViewModel;
                private _undoEngine;
                private _itemsProvider;
                private _applyDataSourceChange;
                private _findDataSource;
                private static _onFail;
                constructor(dsHelper: ko.Observable<DataSourceHelper> | ko.Computed<DataSourceHelper>, dataSourceWizard: Analytics.Wizard.DataSourceWizard, reportViewModel: ko.Observable<Controls.ReportViewModel> | ko.Computed<Controls.ReportViewModel>, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, itemsProvider: ko.Observable<Analytics.Utils.IItemsProvider> | ko.Computed<Analytics.Utils.IItemsProvider>);
                editSchema(dataSourceID: string): void;
                applyDataSourceWizardChanges(dataSourceWizardModel: Analytics.Wizard.IDataSourceWizardState): JQueryPromise<IDataSourceInfo>;
                saveJsonSource(state: Analytics.Wizard.IDataSourceWizardState, connections: Analytics.Wizard.IDataSourceWizardConnectionStrings): JQueryPromise<{}>;
                static createJsonDataSourceInfo(source: Analytics.Data.JsonDataSource): JQueryPromise<IDataSourceInfo>;
                editSchemaAction: {
                    clickAction: (item: any) => void;
                    position: number;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: any;
                };
                getActions(context: any): Analytics.Utils.IAction[];
            }
        }
        module Actions {
            class TableRowActions extends Analytics.Internal.BaseActionsProvider {
                selection: Analytics.Internal.ISelectionProvider;
                readonly _row: Controls.XRTableRowViewModel;
                readonly _table: Controls.XRTableControlViewModel;
                constructor(selection: Analytics.Internal.ISelectionProvider, onComponentAdded?: any);
                insertRowAbove(): void;
                insertRowBelow(): void;
                deleteRow(): void;
                condition(context: any): boolean;
                onComponentAdded: (e: Utils.IComponentAddedEventArgs) => void;
            }
            class TableCellGroupActions extends Analytics.Internal.BaseActionsProvider {
                private _selectionProvider;
                private _distributeColumnsAction;
                private _distributeRowsAction;
                constructor(selectionProvider: Analytics.Internal.ISelectionProvider);
                _distributeColumns(): void;
                _distributeRows(): void;
                _calculateMinimalHeight(cell: Controls.XRTableCellViewModel): number;
                _calculateTextHeight(cell: Controls.XRTableCellViewModel): number;
                _calculateBordersHeight(cell: Controls.XRTableCellViewModel): number;
                _isCellTextControl(cell: Controls.XRTableCellViewModel): boolean;
                _calculatePaddingsHeight(cell: Controls.XRTableCellViewModel): number;
                _calculatePaddingsWidth(cell: Controls.XRTableCellViewModel): number;
                _selectedCells(): any[];
                condition(context: any): boolean;
            }
            class TableCellActions extends TableRowActions {
                readonly _cell: Controls.XRTableCellViewModel;
                readonly _row: Controls.XRTableRowViewModel;
                readonly _table: Controls.XRTableControlViewModel;
                private readonly _cellSurface;
                constructor(selection: Analytics.Internal.ISelectionProvider, onComponentAdded?: any);
                insertCell(): void;
                deleteCell(): void;
                deleteRow(): void;
                insertColumn(isRight: boolean): void;
                deleteColumn(): void;
                condition(context: any): boolean;
                onComponentAdded: (e: Utils.IComponentAddedEventArgs) => void;
            }
            class TextElementAction extends Analytics.Internal.BaseActionsProvider {
                private _selectionProvider;
                private readonly _textControls;
                private _inaccessibleAction;
                constructor(_selectionProvider: Analytics.Internal.ISelectionProvider);
                condition(context: any): boolean;
            }
        }
        module Internal {
            interface IPropertyDescription {
                events: string[];
                group?: string;
                objectProperties?: string[];
            }
            class ExpressionWrapper extends Analytics.Utils.Disposable {
                private _bindingMode;
                private _fieldListProvider?;
                dispose(): void;
                static createExpression(propertyName: any, eventName: any, expression: any): Controls.IExpressionBinding;
                private _valuesDictionary;
                private _displayNameDictionary;
                private _expressionsInfo;
                private _expressionsSerializationInfoCache;
                private _createPropertyByName;
                private _createInfo;
                private _addControlInfo;
                private _createSerializationInfo;
                private _getStateObjKeys;
                private _getExpressionFromArray;
                private _createExpressionMap;
                private _summaryFunctions;
                private _mapExpressionsToObjectByEventName;
                private _validateExpressions;
                private _getExpressionByPropertyName;
                private _mapExpressionsToObject;
                constructor(_bindingMode?: string, _fieldListProvider?: ko.Observable | ko.Computed);
                setPropertyDescription(controlType: string, propertyName: string, events: string[], objectProperties?: string[], group?: string): void;
                hidePropertyDescriptions(controlType: string, ...propertyNames: any[]): void;
                createExpressionsObject(controlType: string, expressions: ko.ObservableArray<Controls.IExpressionBinding>, path?: ko.Observable<string> | ko.Computed<string>, summaryRunning?: (name: string) => ko.Observable<boolean> | ko.Computed<boolean>): IExpressionObject;
                setLocalizationId(propertyName: string, localizationId: string, displayName?: string): void;
                setValues(propertyName: string, values: any[]): void;
            }
            interface IExpressionObject {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                getExpression(propertyName: string, eventName: string): Analytics.Widgets.IExpressionOptions;
                validateExpression(): boolean;
            }
            class WrappedExpressionOptions extends Analytics.Utils.Disposable implements DevExpress.Analytics.Widgets.IExpressionOptions {
                eventName?: string;
                constructor(options: Analytics.Widgets.IExpressionOptions, handlers: {
                    addExpression: (newVal: string) => void;
                    removeExpression: (expression: Controls.IExpressionBinding) => void;
                }, fieldListProvider?: any, eventName?: string);
                isValid: ko.Observable<boolean> | ko.Computed<boolean>;
                expression: ko.Observable<Controls.IExpressionBinding> | ko.Computed<Controls.IExpressionBinding>;
                value: ko.Observable<string> | ko.Computed<string>;
                path: ko.Observable<string> | ko.Computed<string>;
                functions: Array<Analytics.Widgets.Internal.IExpressionEditorFunction>;
                rootItems: ({
                    name: string;
                    needPrefix: boolean;
                    rootPath?: undefined;
                } | {
                    name: string;
                    needPrefix: boolean;
                    rootPath: string;
                })[];
            }
            var createSinglePopularBindingInfos: (propertyName: string) => Analytics.Utils.ISerializationInfoArray;
            var createPopularBindingInfos: (options: Controls.Metadata.ISerializationInfoWithBindings) => Analytics.Utils.ISerializationInfoArray;
        }
        module Utils {
            var createPopularBindingInfo: (options: Controls.Metadata.ISerializationInfoWithBindings, isExpression?: boolean) => Controls.Metadata.ISerializationInfoWithBindings;
            function _isReorderBand(dropTarget: Analytics.Internal.ISelectionTarget, dragFrom: DevExpress.Analytics.Elements.ElementViewModel): boolean;
        }
        module Internal {
            function registerControls(fieldListProvider?: any): void;
        }
        module Utils {
            interface IComponentAddedEventArgs {
                parent: any;
                model: any;
            }
            interface IReportNavigationTabsCustomizationHandler {
                reportTabClosing?: (tab: Tools.INavigateTab, deffered: JQueryDeferred<any>) => boolean;
                reportTabClosed?: (tab: Tools.INavigateTab) => void;
                reportOpening?: (e: any) => void;
                reportOpened?: (e: any) => void;
                tabChanged?: (tab: Tools.INavigateTab) => void;
            }
            interface IReportDesignerCustomizationHandler extends DevExpress.Reporting.Viewer.Utils.IParametersCustomizationHandler, Analytics.Internal.ICommonCustomizationHandler, IReportNavigationTabsCustomizationHandler {
                fieldLists?: (IPathRequest: any) => JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
                exitDesigner?: () => void;
                reportSaving?: (e: any) => void;
                reportSaved?: (e: any) => void;
                customizeParts?: (parts: Analytics.Internal.IDesignerPart[]) => void;
                componentAdded?: (e: IComponentAddedEventArgs) => void;
                customizeSaveDialog?: (popup: Tools.ReportDialogBase) => void;
                customizeOpenDialog?: (popup: Tools.ReportDialogBase) => void;
                customizeWizard?: (wizardType: Wizard.WizardTypeString, wizard: Wizard.WizardType) => void;
                customizeSaveAsDialog?: (popup: Tools.ReportDialogBase) => void;
                customizeToolbox?: (controlsStore: Controls.ControlsFactory) => void;
                customizeFieldListActions?: (fieldListItem: Analytics.Utils.IDataMemberInfo, actions: Analytics.Utils.IAction[]) => void;
            }
            interface IDataSourceRefInfo {
                ref: string;
                name: string;
                isSqlDataSource?: boolean;
                isJsonDataSource?: boolean;
                dataSerializer?: string;
            }
            interface ICultureInfoList {
                csvSeparator?: string;
                fontSet?: Array<string>;
            }
            interface IReportWizardSettings extends Analytics.Wizard.IDataSourceWizardSettings {
                useFullscreenWizard?: boolean;
                useMasterDetailWizard?: boolean;
            }
            class ReportWizardSettings extends Analytics.Wizard.DataSourceWizardSettings implements IReportWizardSettings {
                createDefault(wizardSettings?: IReportWizardSettings): IReportWizardSettings;
                useFullscreenWizard?: boolean;
                useMasterDetailWizard?: boolean;
            }
            interface IWizardConnections {
                sql?: Analytics.Wizard.IConnectionStringDefinition[];
                json?: Analytics.Wizard.IConnectionStringDefinition[];
            }
            interface IReportDesignerInitializationData {
                report: ko.Observable<any>;
                dataBindingMode: DataBindingModeValue;
                convertBindingsToExpressions?: string;
                allowMDI?: boolean;
                canCreateJsonDataSource?: boolean;
                reportUrl: ko.Observable<string> | ko.Computed<string>;
                availableDataSources: Analytics.Internal.IDataSourceInfo[];
                formatStringData?: {
                    standardPatterns: {
                        [key: string]: DevExpress.Analytics.Widgets.Internal.IStandardPattern;
                    };
                    customPatterns: {
                        [key: string]: Array<string>;
                    };
                };
                dataSourceRefs: any[];
                state?: any;
                cultureInfoList?: ICultureInfoList;
                isReportServer?: boolean;
                disableCustomSql: boolean;
                wizardSettings?: IReportWizardSettings;
                wizardConnections?: IWizardConnections;
                isScriptsDisabled?: boolean;
                reportStorageWebIsRegister: boolean;
                subreports?: any;
            }
            interface IReportDesignerInitializationModel extends DevExpress.Analytics.Internal.IGlobalizeSettings {
                reportModel?: any;
                reportModelRootName?: string;
                dataBindingMode?: DataBindingModeValue;
                canCreateJsonDataSource?: boolean;
                convertBindingsToExpressions?: string;
                allowMDI?: boolean;
                formatStringData?: {
                    customPatterns: Array<DevExpress.Reporting.IKeyValuePair<any>>;
                    standardPatterns: Array<DevExpress.Reporting.IKeyValuePair<any>>;
                };
                reportUrl?: string;
                dataSources?: Analytics.Internal.IDataSourceInfo[];
                dataSourcesData?: any[];
                dataSourceRefs?: any[];
                subreports?: any;
                internalSettings?: {
                    isReportServer?: boolean;
                };
                disableCustomSql: boolean;
                scriptsEnabled?: boolean;
                reportStorageWebIsRegister?: boolean;
                cultureInfoList?: ICultureInfoList;
                reportExtensions?: any;
                wizardSettings?: IReportWizardSettings;
                wizardConnections?: IWizardConnections;
                knownEnums?: Array<DevExpress.Reporting.IEnumType>;
                localization?: any;
                rtl?: boolean;
                handlerUri?: string;
                viewerHandlerUri?: string;
                limitation?: boolean;
                queryBuilderHandlerUri?: string;
            }
            enum PaperKind {
                Custom = 0,
                Letter = 1,
                LetterSmall = 2,
                Tabloid = 3,
                Ledger = 4,
                Legal = 5,
                Statement = 6,
                Executive = 7,
                A3 = 8,
                A4 = 9,
                A4Small = 10,
                A5 = 11,
                B4 = 12,
                B5 = 13,
                Folio = 14,
                Quarto = 15,
                Standard10x14 = 16,
                Standard11x17 = 17,
                Note = 18,
                Number9Envelope = 19,
                Number10Envelope = 20,
                Number11Envelope = 21,
                Number12Envelope = 22,
                Number14Envelope = 23,
                CSheet = 24,
                DSheet = 25,
                ESheet = 26,
                DLEnvelope = 27,
                C5Envelope = 28,
                C3Envelope = 29,
                C4Envelope = 30,
                C6Envelope = 31,
                C65Envelope = 32,
                B4Envelope = 33,
                B5Envelope = 34,
                B6Envelope = 35,
                ItalyEnvelope = 36,
                MonarchEnvelope = 37,
                PersonalEnvelope = 38,
                USStandardFanfold = 39,
                GermanStandardFanfold = 40,
                GermanLegalFanfold = 41,
                IsoB4 = 42,
                JapanesePostcard = 43,
                Standard9x11 = 44,
                Standard10x11 = 45,
                Standard15x11 = 46,
                InviteEnvelope = 47,
                LetterExtra = 50,
                LegalExtra = 51,
                TabloidExtra = 52,
                A4Extra = 53,
                LetterTransverse = 54,
                A4Transverse = 55,
                LetterExtraTransverse = 56,
                APlus = 57,
                BPlus = 58,
                LetterPlus = 59,
                A4Plus = 60,
                A5Transverse = 61,
                B5Transverse = 62,
                A3Extra = 63,
                A5Extra = 64,
                B5Extra = 65,
                A2 = 66,
                A3Transverse = 67,
                A3ExtraTransverse = 68,
                JapaneseDoublePostcard = 69,
                A6 = 70,
                JapaneseEnvelopeKakuNumber2 = 71,
                JapaneseEnvelopeKakuNumber3 = 72,
                JapaneseEnvelopeChouNumber3 = 73,
                JapaneseEnvelopeChouNumber4 = 74,
                LetterRotated = 75,
                A3Rotated = 76,
                A4Rotated = 77,
                A5Rotated = 78,
                B4JisRotated = 79,
                B5JisRotated = 80,
                JapanesePostcardRotated = 81,
                JapaneseDoublePostcardRotated = 82,
                A6Rotated = 83,
                JapaneseEnvelopeKakuNumber2Rotated = 84,
                JapaneseEnvelopeKakuNumber3Rotated = 85,
                JapaneseEnvelopeChouNumber3Rotated = 86,
                JapaneseEnvelopeChouNumber4Rotated = 87,
                B6Jis = 88,
                B6JisRotated = 89,
                Standard12x11 = 90,
                JapaneseEnvelopeYouNumber4 = 91,
                JapaneseEnvelopeYouNumber4Rotated = 92,
                Prc16K = 93,
                Prc32K = 94,
                Prc32KBig = 95,
                PrcEnvelopeNumber1 = 96,
                PrcEnvelopeNumber2 = 97,
                PrcEnvelopeNumber3 = 98,
                PrcEnvelopeNumber4 = 99,
                PrcEnvelopeNumber5 = 100,
                PrcEnvelopeNumber6 = 101,
                PrcEnvelopeNumber7 = 102,
                PrcEnvelopeNumber8 = 103,
                PrcEnvelopeNumber9 = 104,
                PrcEnvelopeNumber10 = 105,
                Prc16KRotated = 106,
                Prc32KRotated = 107,
                Prc32KBigRotated = 108,
                PrcEnvelopeNumber1Rotated = 109,
                PrcEnvelopeNumber2Rotated = 110,
                PrcEnvelopeNumber3Rotated = 111,
                PrcEnvelopeNumber4Rotated = 112,
                PrcEnvelopeNumber5Rotated = 113,
                PrcEnvelopeNumber6Rotated = 114,
                PrcEnvelopeNumber7Rotated = 115,
                PrcEnvelopeNumber8Rotated = 116,
                PrcEnvelopeNumber9Rotated = 117,
                PrcEnvelopeNumber10Rotated = 118
            }
            var ReportDesignerElements: any;
            var ReportDesignerAddOns: {
                Preview: string;
                ReportWizard: string;
                ReportWizardFullscreen: string;
                DataSourceWizard: string;
                MultiQueryDataSourceWizard: string;
                MultiQueryDataSourceWizardFullscreen: string;
                MasterDetailEditor: string;
                ScriptEditor: string;
            };
            type DataBindingModeValue = "Bindings" | "Expressions" | "ExpressionsAdvanced";
            var controlsFactory: Controls.ControlsFactory;
            var DataBindingMode: DataBindingModeValue;
            var HandlerUri: string;
            var formatStringEditorCustomSet: {
                [key: string]: string[];
            };
        }
        module Widgets {
            class BandEditorBase extends Analytics.Widgets.Editor {
                generateValue: (bands: (filter?: any, noneNeaded?: any) => ko.Computed<any>) => any;
                bands: any;
                filter: (item: Bands.BandViewModel) => boolean;
                noneNeaded: boolean;
            }
            class RunningBandEditor extends BandEditorBase {
                filter: (item: Bands.BandViewModel) => boolean;
                noneNeaded: boolean;
            }
            class BandsEditor extends BandEditorBase {
                filter: (item: Bands.BandViewModel) => boolean;
                noneNeaded: boolean;
            }
            class SortingBandEditor extends BandEditorBase {
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                filter: (item: Bands.BandViewModel) => boolean;
                noneNeaded: boolean;
            }
            class DataSourceEditor extends Analytics.Widgets.Editor {
                private _getEditorOptions;
                dispose(): void;
                getEditorOptions(dataSourceHelper: ko.Observable<Reporting.Designer.Internal.DataSourceHelper>, undoEngine: ko.Observable<Analytics.Utils.UndoEngine>, popupContainer: string): any;
            }
            class ChartValueBindingEditor extends Analytics.Widgets.Editor {
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                generateDisplayValue(reportDataSource: ko.Observable<Analytics.Internal.IDataSourceInfo>): string;
                generateValue(undoEngine: Analytics.Utils.UndoEngine, reportParameters: ko.ObservableArray<Data.Parameter>, reportDataSource: ko.Observable<Analytics.Internal.IDataSourceInfo>): ko.Observable<string> | ko.Computed<string>;
                treeListController: Analytics.Widgets.Internal.TreeListController;
                binding: ko.Observable<string> | ko.Computed<string>;
                displayBinding: ko.Observable<string> | ko.Computed<string>;
            }
            var reportFunctionDisplay: Analytics.Widgets.Internal.IExpressionEditorFunction[];
            class ReportExpressionEditor extends DevExpress.Analytics.Widgets.Editor {
                constructor(modelPropertyInfo: any, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                private _createReportItems;
                private _createValuesTab;
                patchOptions(reportExplorerProvider: any): boolean;
                value: ko.Computed<Designer.Internal.WrappedExpressionOptions>;
            }
            class ExplorerEditor extends DevExpress.Analytics.Widgets.Editor {
                constructor(modelPropertyInfo: any, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                private _collectionNames;
                private _isVisible;
                treeListController: Analytics.Internal.ObjectStructureTreeListController;
                displayExpr: ko.Observable<string> | ko.Computed<string>;
                itemsProvider: Analytics.Internal.ObjectExplorerProvider;
            }
            class DrillDownEditor extends ExplorerEditor {
                private _setDisabled;
                private _findFistAvailableBand;
                constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                path: ko.Observable<any>;
            }
            class EditOptionsEditorNameEditorModel extends Analytics.Widgets.Editor {
                constructor(modelPropertyInfo: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Observable<boolean>, textToSearch?: any);
                itemsProvider: Analytics.Utils.IItemsProvider;
                displayValue: ko.Observable<string>;
            }
            class FieldsComboboxEditor extends Analytics.Widgets.FieldListEditor {
                private _createItem;
                private _updateValues;
                wrapValues(displayNameProvider: ko.Observable<Analytics.Utils.IDisplayNameProvider>): any;
                wrappedValues: any;
            }
            class FormattingRuleEditor extends Analytics.Widgets.Editor {
                constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                options: Analytics.Widgets.Internal.ICollectionEditorOptions;
            }
            class GaugeStyleEditor extends Analytics.Widgets.Editor {
                private _viewModel;
                constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                update(viewModel: any): void;
            }
            class dxImageSourceEditor extends Analytics.Widgets.Internal.dxFileImagePicker {
                constructor(element: any, options?: any);
                _toggleReadOnlyState(): void;
                _handleFiles(filesHolder: {
                    files: any;
                }): void;
            }
            class LinesEditor extends DevExpress.Analytics.Widgets.Editor {
                collapsed: ko.Observable<boolean>;
            }
            class NameEditor extends Analytics.Widgets.Editor {
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                _getEditorValidationRules(): any;
                generateRules(allControls: ko.ObservableArray<Analytics.Internal.IDisplayedObject>): any;
                currentValidationRules: any;
            }
            class PivotGridCriteriaEditor extends Analytics.Widgets.Editor {
                private _createItemsProvider;
                private _getFieldName;
                private _createDisplayNameProvider;
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                wrapModel(fieldListProvider: any): this;
                itemsProvider: Analytics.Utils.IItemsProvider;
                displayNameProvider: Analytics.Utils.IDisplayNameProvider;
            }
            class ContentByTypeEditor extends Analytics.Widgets.PropertyGridEditor {
                createObjectProperties(): Analytics.Widgets.ObjectProperties;
                getViewModel(): ko.Computed<any>;
            }
            class DataBindingsEditor extends Analytics.Widgets.PropertyGridEditor {
                constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                createObjectProperties(): Analytics.Widgets.ObjectProperties;
            }
            class FontEditorUndo extends Analytics.Widgets.PropertyGridEditor {
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                generateValue(undoEngine: ko.Observable<Analytics.Utils.UndoEngine>): any;
                createObjectProperties(): Analytics.Widgets.ObjectProperties;
                undoEngine: ko.Observable<Analytics.Utils.UndoEngine>;
            }
            class RichTextFileEditor extends Analytics.Widgets.Editor {
                private _toStreamType;
                constructor(info: Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                format: ko.Observable<string>;
                _model: ko.Observable<Controls.XRRichViewModel>;
            }
            class UndoEditor extends Analytics.Widgets.Editor {
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                generateValue(undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>): ko.Computed<any> | ko.Observable<any>;
                generatedValue: ko.Observable | ko.Computed;
            }
            class ComboboxUndoEditor extends Analytics.Widgets.Editor {
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                generateValue(undoEngine: ko.Observable<Analytics.Utils.UndoEngine>): ko.Computed<any> | ko.Observable<any>;
                undoValue: ko.Observable | ko.Computed;
            }
            class ReportUrlEditor extends Analytics.Widgets.Editor {
                constructor(info: DevExpress.Analytics.Utils.ISerializationInfo, level: any, parentDisabled?: ko.Computed<boolean>, textToSearch?: any);
                getItems(urls: ko.ObservableArray<IKeyValuePair<string>>, tab: ko.Observable<DevExpress.Reporting.Designer.Tools.INavigateTab>): ko.Computed<IKeyValuePair<string>[]>;
                urls: ko.Computed<IKeyValuePair<string>[]>;
            }
            class StylesEditorHeaderModel {
                static newItem: string;
                static newItemTextId: string;
                constructor(styleName: ko.Observable<string>, styles: ko.ObservableArray<Controls.StyleModel>, disabled: ko.Observable<boolean>, popupContainer: string);
                items: ko.Computed<Controls.StyleModel[]>;
                value: any;
                onValueChanged: (e: any) => void;
                displayExpr: string;
                valueExpr: string;
                displayCustomValue: boolean;
                placeholder: any;
                noDataText: any;
                disabled: ko.Observable<boolean> | ko.Computed<boolean>;
                dropDownOptions: any;
            }
            var editorTemplates: {
                dataSource: {
                    header: string;
                    editorType: typeof DataSourceEditor;
                };
                dataBindings: {
                    header: string;
                    content: string;
                    editorType: typeof DataBindingsEditor;
                };
                dataBinding: {
                    header: string;
                    content: string;
                    editorType: typeof Analytics.Widgets.FieldListEditor;
                };
                reportExplorer: {
                    header: string;
                    editorType: typeof ExplorerEditor;
                };
                reportSourceUrl: {
                    header: string;
                    editorType: typeof ReportUrlEditor;
                };
                bands: {
                    header: string;
                    editorType: typeof BandsEditor;
                };
                runningBand: {
                    header: string;
                    editorType: typeof RunningBandEditor;
                };
                sortingBand: {
                    header: string;
                    content: string;
                    editorType: typeof SortingBandEditor;
                };
                style: {
                    header: string;
                    content: string;
                };
                stylePriority: {
                    header: string;
                };
                contentByType: {
                    header: string;
                    content: string;
                    editorType: typeof ContentByTypeEditor;
                };
                lookUpValues: {
                    custom: string;
                };
                reportexpression: {
                    header: string;
                    editorType: typeof ReportExpressionEditor;
                };
                drillDownControls: {
                    header: string;
                    editorType: typeof DrillDownEditor;
                };
                viewStyle: {
                    header: string;
                    editorType: typeof GaugeStyleEditor;
                };
                pivotGridFields: {
                    custom: string;
                };
                scriptsBox: {
                    header: string;
                };
                formattingRule: {
                    custom: string;
                    editorType: typeof FormattingRuleEditor;
                };
                toclevel: {
                    custom: string;
                };
                calculatedFields: {
                    custom: string;
                };
                parameters: {
                    custom: string;
                };
                reportRtlProperty: {
                    header: string;
                };
                comboboxUndo: {
                    header: string;
                    editorType: typeof ComboboxUndoEditor;
                };
                fontUndo: {
                    header: string;
                    content: string;
                    editorType: typeof FontEditorUndo;
                };
                chartValueBinding: {
                    header: string;
                    editorType: typeof ChartValueBindingEditor;
                };
                name: {
                    header: string;
                    editorType: typeof NameEditor;
                };
                numericUndo: {
                    header: string;
                    editorType: typeof UndoEditor;
                };
                pivotCriteria: {
                    header: string;
                    editorType: typeof PivotGridCriteriaEditor;
                };
                fieldsCombobox: {
                    header: string;
                    editorType: typeof FieldsComboboxEditor;
                };
                richTextLoad: {
                    header: string;
                    editorType: typeof RichTextFileEditor;
                };
                summaryEditor: {
                    header: string;
                    content: string;
                    editorType: any;
                };
            };
            module Internal {
                var dataBindingsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            module Internal {
                interface ISummaryOptions {
                    ignoreNullValues: ko.Observable<boolean> | ko.Computed<boolean>;
                    treatStringsAsNumerics: ko.Observable<boolean> | ko.Computed<boolean>;
                    Running: ko.Observable<string> | ko.Computed<string>;
                }
                class SummaryEditorPopup {
                    dispose(): void;
                    model: ko.Observable<SummaryEditorModel>;
                    grid: Analytics.Widgets.ObjectProperties;
                    visible: ko.Observable<boolean>;
                    container: (element: HTMLElement) => JQuery;
                    buttons: {
                        toolbar: string;
                        location: string;
                        widget: string;
                        options: {
                            text: any;
                            onClick: () => void;
                        };
                    }[];
                }
                class SummaryEditorModel extends Analytics.Utils.Disposable {
                    private _control;
                    dispose(): void;
                    private _summary;
                    private _order;
                    private _summaryFunctionValues;
                    private _info;
                    private _initExpressionValues;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    constructor(_control: Controls.XRControlViewModel);
                    applyChanges(): void;
                    isPropertyDisabled(propertyName: string): boolean;
                    isDisabled(): boolean;
                    Func: ko.Observable<string>;
                    calculate: Analytics.Widgets.IExpressionOptions;
                    weight: Analytics.Widgets.IExpressionOptions;
                    ignoreNullValues: ko.Observable<boolean> | ko.Computed<boolean>;
                    treatStringsAsNumerics: ko.Observable<boolean> | ko.Computed<boolean>;
                    Running: ko.Observable<string> | ko.Computed<string>;
                }
            }
            class SummaryEditor extends Analytics.Widgets.PropertyGridEditor {
                dispose(): void;
                getPopupServiceActions(): any[];
                summaryModel: Internal.SummaryEditorModel;
                popup: Internal.SummaryEditorPopup;
            }
        }
        module Controls {
            module Metadata {
                var afterPrint: Analytics.Utils.ISerializationInfo;
                var beforePrint: Analytics.Utils.ISerializationInfo;
                var sizeChanged: Analytics.Utils.ISerializationInfo;
                var truncatedControlEventsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var truncatedControlScripts: Analytics.Utils.ISerializationInfo;
                var commonScripts: Analytics.Utils.ISerializationInfo;
                var controlScripts: Analytics.Utils.ISerializationInfo;
                var textControlScripts: Analytics.Utils.ISerializationInfo;
                var labelScripts: Analytics.Utils.ISerializationInfo;
                var chartScripts: Analytics.Utils.ISerializationInfo;
                var pivotScripts: Analytics.Utils.ISerializationInfo;
                var subreportScripts: Analytics.Utils.ISerializationInfo;
                var commonBandScripts: Analytics.Utils.ISerializationInfo;
                var groupBandScripts: Analytics.Utils.ISerializationInfo;
                var groupHeaderBandScripts: Analytics.Utils.ISerializationInfo;
                var detailReportBandScripts: Analytics.Utils.ISerializationInfo;
                var reportScripts: Analytics.Utils.ISerializationInfo;
                var allScripts: Analytics.Utils.ISerializationInfo;
                interface ISerializationInfoWithBindings extends Analytics.Utils.ISerializationInfo {
                    bindingName?: string;
                }
                var textAlignmentValues: {
                    value: string;
                    displayValue: string;
                    localizationId: string;
                }[];
                var borderDashStyleValues: {
                    value: string;
                    displayValue: string;
                    localizationId: string;
                }[];
                var stylePrioritySerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var xlsxFormatString: Analytics.Utils.ISerializationInfo;
                var name: Analytics.Utils.ISerializationInfo;
                var displayName: Analytics.Utils.ISerializationInfo;
                var text: Analytics.Utils.ISerializationInfo;
                var textArea: Analytics.Utils.ISerializationInfo;
                var textTrimming: Analytics.Utils.ISerializationInfo;
                var size: Analytics.Utils.ISerializationInfo;
                var location: Analytics.Utils.ISerializationInfo;
                var defaultBooleanValuesArray: Array<Analytics.Utils.IDisplayedValue>;
                var tag: Analytics.Utils.ISerializationInfo;
                var lockedInUserDesigner: Analytics.Utils.ISerializationInfo;
                var visible: Analytics.Utils.ISerializationInfo;
                var backColor: Analytics.Utils.ISerializationInfo;
                var foreColor: Analytics.Utils.ISerializationInfo;
                var font: Analytics.Utils.ISerializationInfo;
                var borderColor: Analytics.Utils.ISerializationInfo;
                var borders: Analytics.Utils.ISerializationInfo;
                var borderWidth: Analytics.Utils.ISerializationInfo;
                var borderDashStyle: Analytics.Utils.ISerializationInfo;
                var paddingString: Analytics.Utils.ISerializationInfo;
                var padding: Analytics.Utils.ISerializationInfo;
                var textAlignment: Analytics.Utils.ISerializationInfo;
                var textFitMode: Analytics.Utils.ISerializationInfo;
                var angle: Analytics.Utils.ISerializationInfo;
                var canGrow: Analytics.Utils.ISerializationInfo;
                var canShrink: Analytics.Utils.ISerializationInfo;
                var multiline: Analytics.Utils.ISerializationInfo;
                var wordWrap: Analytics.Utils.ISerializationInfo;
                var allowMarkupText: Analytics.Utils.ISerializationInfo;
                var autoWidth: Analytics.Utils.ISerializationInfo;
                var keepTogether: Analytics.Utils.ISerializationInfo;
                var keepTogetherDefaultValueFalse: Analytics.Utils.ISerializationInfo;
                var processDuplicatesTarget: Analytics.Utils.ISerializationInfo;
                var processDuplicatesMode: Analytics.Utils.ISerializationInfo;
                var processNullValues: Analytics.Utils.ISerializationInfo;
                var reportPrintOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var dataAdapter: Analytics.Utils.ISerializationInfo;
                var dataSource: Analytics.Utils.ISerializationInfo;
                var dataMember: Analytics.Utils.ISerializationInfo;
                var filterString: Analytics.Utils.ISerializationInfo;
                var filterStringEditable: Analytics.Utils.ISerializationInfo;
                var bookmark: Analytics.Utils.ISerializationInfo;
                var bookmarkParent: Analytics.Utils.ISerializationInfo;
                var navigateUrl: Analytics.Utils.ISerializationInfo;
                var target: Analytics.Utils.ISerializationInfo;
                var nullValueText: Analytics.Utils.ISerializationInfo;
                function getSummaryFunctionValues(): Array<Analytics.Utils.IDisplayedValue>;
                var summaryFunctionValues: Array<Analytics.Utils.IDisplayedValue>;
                var textFormatString: Analytics.Utils.ISerializationInfo;
                function createSummarySerializationInfo(summaryFunctions?: Analytics.Utils.IDisplayedValue[]): Analytics.Utils.ISerializationInfoArray;
                var summarySerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var summary: Analytics.Utils.ISerializationInfo;
                var reportPrintOptions: Analytics.Utils.ISerializationInfo;
                var lineWidth: Analytics.Utils.ISerializationInfo;
                var lineStyle: Analytics.Utils.ISerializationInfo;
                var dpi: Analytics.Utils.ISerializationInfo;
                var canPublish: Analytics.Utils.ISerializationInfo;
                var rtl: Analytics.Utils.ISerializationInfo;
                var imageType: Analytics.Utils.ISerializationInfo;
                var paddingGroup: Analytics.Utils.ISerializationInfo[];
            }
        }
        module Data {
            class DataBindingBase extends Analytics.Utils.Disposable {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                private _findDataSourceFromPath;
                updateParameter(pathRequest: DevExpress.Analytics.Utils.PathRequest, dataSources: Analytics.Internal.IDataSourceInfo[]): void;
                updateBinding(path: string, dataSources: any): void;
                getValuePath(dataSourceHelper: any): string;
                generateValue(undoEngine: DevExpress.Analytics.Utils.UndoEngine, dataSourceHelper: Internal.DataSourceHelper, dataSources: any): ko.Computed<string>;
                resetValue(): void;
                isEmpty(): boolean;
                value: ko.Observable<string> | ko.Computed<string>;
                generatedValue: ko.Computed<string>;
                parameter: ko.Observable<Parameter> | ko.Computed<Parameter>;
                dataSource: ko.Observable<ObjectStorageItem> | ko.Computed<ObjectStorageItem>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                displayExpr: ko.Computed<string>;
            }
            class DataBinding extends DataBindingBase {
                static initialize(model: any, serializer?: Analytics.Utils.ModelSerializer): ko.ObservableArray<DataBinding>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                updateParameter(pathRequest: DevExpress.Analytics.Utils.PathRequest, dataSources: any): void;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                resetValue(): void;
                visible: ko.Observable<boolean>;
                disabled: ko.PureComputed<boolean>;
                propertyName: ko.Observable<string> | ko.Computed<string>;
                formatString: ko.Observable<string> | ko.Computed<string>;
            }
            module Metadata {
                var dataBindingBaseSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var dataBindingSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var dataBindings: (dataBindingsArray: string[]) => Analytics.Utils.ISerializationInfo;
            }
        }
        module Controls {
            class FormattingRule extends Analytics.Utils.Disposable {
                static createNew(report?: any): FormattingRule;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, parent: ReportViewModel, serializer?: Analytics.Utils.IModelSerializer);
                getPath(propertyName: any): string;
                className: () => string;
                displayType(): any;
                controlType: string;
                selected: ko.Observable<boolean> | ko.Computed<boolean>;
                name: ko.Observable<string> | ko.Computed<string>;
                parent: ReportViewModel;
                dataSource: ko.Observable<Data.ObjectStorageItem> | ko.Computed<Data.ObjectStorageItem>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                condition: ko.Observable<string> | ko.Computed<string>;
            }
            class FormattingRuleLink {
                static createNew(rule: FormattingRule): FormattingRuleLink;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                value: ko.Observable<FormattingRule> | ko.Computed<FormattingRule>;
            }
            module Metadata {
                var formattingRuleLinkSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var formattingRuleLinks: Analytics.Utils.ISerializationInfo;
                var defaultBooleanVisible: Analytics.Utils.ISerializationInfo;
                var formattingSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var conditionObj: Analytics.Utils.ISerializationInfo;
                var formatting: Analytics.Utils.ISerializationInfo;
                var formattingRuleSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            interface IAnchoringProperties {
                size?: Analytics.Elements.ISize;
                location?: Analytics.Elements.IPoint;
                root: any;
            }
            class Anchoring extends Analytics.Utils.Disposable {
                static states: {
                    inProgress: string;
                    complete: string;
                    fromControls: string;
                };
                anchoring: ko.Observable<string> | ko.Computed<string>;
                subscribtion: ko.Subscription;
                dispose(): void;
                constructor(subscrible: ko.Observable<number> | ko.Computed<number>, model: IAnchoringProperties, anchoringProperty: ko.Observable<string> | ko.Computed<string>);
                start(subscrible: ko.Observable<number> | ko.Computed<number>, model: IAnchoringProperties): void;
                anchorSubscribtion: (parentSizeValue: number, oldValue: ko.Observable<number> | ko.Computed<number>, model: IAnchoringProperties) => void;
                state: string;
            }
            class VerticalAcnhoring extends Anchoring {
                anchorSubscribtion: (parentSizeValue: number, oldValue: ko.Observable<number> | ko.Computed<number>, model: IAnchoringProperties) => void;
                constructor(subscrible: ko.Observable<number> | ko.Computed<number>, model: IAnchoringProperties, anchoringProperty: ko.Observable<string> | ko.Computed<string>);
            }
            class HorizontalAnchoring extends Anchoring {
                anchorSubscribtion: (parentSizeValue: number, oldValue: ko.Observable<number> | ko.Computed<number>, model: IAnchoringProperties) => void;
                constructor(subscrible: ko.Observable<number> | ko.Computed<number>, model: IAnchoringProperties, anchoringProperty: ko.Observable<string> | ko.Computed<string>);
            }
            module Metadata {
                var anchorVertical: Analytics.Utils.ISerializationInfo;
                var anchorHorizontal: Analytics.Utils.ISerializationInfo;
            }
            class EditOptions implements Analytics.Utils.ISerializableModel {
                id: ko.Observable<string> | ko.Computed<string>;
                enabled: ko.Observable<boolean> | ko.Computed<boolean>;
                constructor(model: {}, serializer?: Analytics.Utils.IModelSerializer);
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                isEmpty(): boolean;
                isPropertyDisabled(name: string): boolean;
            }
            class CheckEditOptions extends EditOptions {
                groupID: ko.Observable<string> | ko.Computed<string>;
                constructor(model: {}, serializer?: Analytics.Utils.IModelSerializer);
                getInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class ImageEditOptions extends EditOptions {
                editorName: ko.Observable<string> | ko.Computed<string>;
                constructor(model: {}, serializer?: Analytics.Utils.IModelSerializer);
                getInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            class TextEditOptions extends EditOptions {
                editorName: ko.Observable<string> | ko.Computed<string>;
                constructor(model: {}, serializer?: Analytics.Utils.IModelSerializer);
                getInfo(): Analytics.Utils.ISerializationInfoArray;
            }
            module Metadata {
                var editOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var editOptions: Analytics.Utils.ISerializationInfo;
                var textEditOptions: Analytics.Utils.ISerializationInfo;
            }
            interface ICheckBoxCustomGlyphs {
                Checked: ko.Observable<DevExpress.Reporting.ImageSource>;
                Unchecked: ko.Observable<DevExpress.Reporting.ImageSource>;
                Indeterminate: ko.Observable<DevExpress.Reporting.ImageSource>;
            }
            function getDefaultCheckSize(checkState?: Viewer.Editing.GlyphStyle): Analytics.Elements.Size;
            class GlyphOptions extends Analytics.Utils.Disposable implements Analytics.Utils.ISerializableModel {
                constructor(model: {}, serializer?: Analytics.Utils.IModelSerializer);
                getInfo: ko.Observable<Analytics.Utils.ISerializationInfoArray>;
                alignment: ko.Observable<string> | ko.Computed<string>;
                size: Analytics.Elements.Size;
                style: ko.Observable<string> | ko.Computed<string>;
                customGlyphs: ICheckBoxCustomGlyphs;
            }
            module Metadata {
                var glyphAlignment: Analytics.Utils.ISerializationInfo;
                var glyphOptionsSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            interface IExpressionBinding {
                eventName: ko.Observable<string>;
                propertyName: ko.Observable<string>;
                expression: ko.Observable<string>;
            }
            module Metadata {
                var expressionBindingSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var expressionBindings: Analytics.Utils.ISerializationInfo;
            }
            class SortingOptions extends Analytics.Utils.Disposable implements Analytics.Utils.ISerializableModel {
                private _info;
                private _fieldNameInfo;
                targetBand: ko.Observable<Bands.BandViewModel>;
                fieldName: ko.Observable<string> | ko.Computed<string>;
                private _getFieldNames;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyDisabled(name: string): boolean;
                resetValue(): void;
                constructor(model: {}, report: ReportViewModel, serializer?: Analytics.Utils.IModelSerializer);
                getPath(propertyName: string): any;
            }
            module Metadata {
                var sortingOptionsSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var interactiveSorting: Analytics.Utils.ISerializationInfo;
            }
            class StyleModel extends Analytics.Utils.Disposable {
                static defaults: {
                    "backColor": string;
                    "foreColor": string;
                    "borderColor": string;
                };
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                isPropertyModified(name: any): boolean;
                className: () => string;
                displayType(): any;
                name: ko.Observable<string> | ko.Computed<string>;
                paddingObj: Analytics.Elements.PaddingModel;
                padding: ko.Observable<any>;
                controlType: string;
            }
            module Metadata {
                var styleSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var styleName: Analytics.Utils.ISerializationInfo;
                var evenStyleName: Analytics.Utils.ISerializationInfo;
                var oddStyleName: Analytics.Utils.ISerializationInfo;
                var stylePriority: Analytics.Utils.ISerializationInfo;
                var pivotGridStyles: Analytics.Utils.ISerializationInfoArray;
                var stylesInfo: Analytics.Utils.ISerializationInfoArray;
                var stylesObj: Analytics.Utils.ISerializationInfo;
            }
            module Metadata {
                var sizeLocation: Analytics.Utils.ISerializationInfoArray;
                var bordersProperties: Analytics.Utils.ISerializationInfoArray;
                var baseControlProperties: Analytics.Utils.ISerializationInfoArray;
                var commonBandProperties: Analytics.Utils.ISerializationInfoArray;
                var commonControlProperties: Analytics.Utils.ISerializationInfoArray;
                var fontGroup: Analytics.Utils.ISerializationInfoArray;
                var bookmarkGroup: Analytics.Utils.ISerializationInfoArray;
                var navigationGroup: Analytics.Utils.ISerializationInfoArray;
                var datasourcePrintOptionsGroup: Analytics.Utils.ISerializationInfoArray;
                var processGroup: Analytics.Utils.ISerializationInfoArray;
                var canGrowShrinkGroup: Analytics.Utils.ISerializationInfoArray;
                var labelGroup: Analytics.Utils.ISerializationInfoArray;
                var unknownSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class ComponentsModel extends Analytics.Utils.Disposable {
                renameComponentStrategy: Internal.IRenameComponentStrategy;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: Analytics.Internal.IDataSourceInfo, renameComponentStrategy: Internal.IRenameComponentStrategy);
                className: () => string;
                controlType: string;
                name: ko.Observable<string> | ko.Computed<string>;
                data: Data.ObjectItem;
            }
            class ExtensionModel {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                key: ko.Observable<string> | ko.Computed<string>;
                value: ko.Observable<string> | ko.Computed<string>;
            }
        }
        module Data {
            class LookUpValue {
                static createNew(): LookUpValue;
                static from(model: any, serializer: Analytics.Utils.ModelSerializer): LookUpValue;
                static toJson(value: any, serializer: any, refs: any): any;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                description: ko.Observable<string> | ko.Computed<string>;
                _value: ko.Observable<ObjectStorageItem> | ko.Computed<ObjectStorageItem>;
                value: ko.Computed<any>;
                valueInfo: ko.Observable<Analytics.Utils.ISerializationInfo> | ko.Computed<Analytics.Utils.ISerializationInfo>;
                readonly isEmpty: boolean;
            }
            module Metadata {
                var lookUpValueSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
        }
        class ReportParameterHelper extends DevExpress.Reporting.Viewer.Parameters.ParameterHelper {
        }
        module Data {
            interface IParameterType {
                value: string;
                displayValue: string;
                defaultVal: any;
                specifics: string;
                valueConverter: (val: any) => any;
            }
            interface IParameterTypeValue {
                value: string;
                displayValue: string;
                defaultValue: any;
                specifics: string;
                valueConverter: (val: any) => any;
                icon?: string;
                localizationId?: string;
            }
            class Parameter extends Analytics.Utils.Disposable implements DevExpress.Reporting.Viewer.Parameters.IParameter, DevExpress.Analytics.Utils.IDataMemberInfo {
                static separator: string;
                static defaultGuidValue: string;
                private _parameterHelper;
                static valueToJsonObject(value: any): any;
                static typeValues: IParameterTypeValue[];
                private _getTypeValue;
                private _initializeLookUpSourceType;
                private _tryConvertValue;
                private _convertSingleValue;
                private _initializeValue;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, report: Controls.ReportViewModel, objectsStorage: ObjectsStorage, parameterHelper: ReportParameterHelper, serializer?: Analytics.Utils.IModelSerializer);
                isPropertyVisible(name: string): boolean;
                initializeLookUpValue(lookUpValue: LookUpValue): void;
                updateLookUpValues(newType: string, value?: any): void;
                getParameterDescriptor(): Viewer.Parameters.IParameterDescriptor;
                readonly name: string;
                readonly specifics: string;
                readonly icon: string;
                readonly defaultValue: any;
                readonly displayName: string;
                templateName: string;
                contenttemplate: string;
                _type: ko.Observable<ObjectStorageItem> | ko.Computed<ObjectStorageItem>;
                _obsoleteValue: ko.Observable | ko.Computed;
                objectsStorage: ObjectsStorage;
                lookUpSettings: ko.Observable<LookUpSettings> | ko.Computed<LookUpSettings>;
                parameterName: ko.Observable<string> | ko.Computed<string>;
                description: ko.Observable<string> | ko.Computed<string>;
                tag: ko.Observable | ko.Computed;
                type: ko.Computed<string>;
                collapsed: ko.Observable<boolean> | ko.Computed<boolean>;
                lookUpSourceType: ko.Observable<string> | ko.Computed<string>;
                visible: ko.Observable<boolean> | ko.Computed<boolean>;
                value: ko.Observable | ko.Computed;
                valueInfo: ko.Observable<Analytics.Utils.ISerializationInfo> | ko.Computed<Analytics.Utils.ISerializationInfo>;
                isMultiValue: ko.Observable<boolean> | ko.Computed<boolean>;
                allowNull: ko.Observable<boolean> | ko.Computed<boolean>;
                multiValueInfo: ko.Observable<Analytics.Utils.ISerializationInfo> | ko.Computed<Analytics.Utils.ISerializationInfo>;
                viewmodel: Analytics.Widgets.ObjectProperties;
            }
            module Metadata {
                var parameterValueSerializationInfo: Analytics.Utils.ISerializationInfo;
                var parameterLookUpSettingsSerializationInfo: Analytics.Utils.ISerializationInfo;
                var parameterSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class CalculatedField extends Analytics.Utils.Disposable implements DevExpress.Analytics.Utils.IDataMemberInfo {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                readonly displayName: any;
                readonly name: string;
                readonly specifics: string;
                readonly type: string;
                templateName: string;
                contenttemplate: string;
                isList: boolean;
                isCalculated: boolean;
                calculatedFieldName: ko.Observable<string> | ko.Computed<string>;
                nameEditable: ko.Computed<string>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                dataSource: ko.Observable<ObjectStorageItem>;
                fieldType: ko.Observable<string> | ko.Computed<string>;
                expressionObj: Analytics.Widgets.IExpressionOptions;
                propertyGrid: Analytics.Widgets.ObjectProperties;
                pathRequest: DevExpress.Analytics.Utils.IPathRequest;
            }
            module Metadata {
                var calculatedFieldScripts: Analytics.Utils.ISerializationInfo;
                var calculatedFieldSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class ObjectItem extends Analytics.Utils.Disposable {
                dsHelperProvider?: () => Internal.DataSourceHelper;
                dispose(): void;
                static createNew(model: any, dsHelperProvider?: () => Internal.DataSourceHelper, serializer?: Analytics.Utils.IModelSerializer): ObjectItem;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                afterDeserialization(model: any, serializer: any): void;
                preInitProperties(model: any, dsHelperProvider?: () => Internal.DataSourceHelper, serializer?: Analytics.Utils.IModelSerializer): void;
                constructor(model: any, dsHelperProvider?: () => Internal.DataSourceHelper, serializer?: Analytics.Utils.IModelSerializer);
                objectType: ko.Observable<string> | ko.Computed<string>;
                clone(): ObjectItem;
            }
            class ObjectStorageItem extends ObjectItem {
                _getInfo(): Analytics.Utils.ISerializationInfoArray;
                preInitProperties(model: any): void;
                constructor(model: any, dsHelperProvider?: any, serializer?: Analytics.Utils.IModelSerializer);
                isEmpty(): boolean;
                content: ko.Observable<string> | ko.Computed<string>;
                type: ko.Observable<string> | ko.Computed<string>;
            }
            class ObjectStorageParameter extends Analytics.Elements.SerializableModel {
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
            }
            class DataFederationDataSource extends ObjectStorageItem {
                preInitProperties(): void;
            }
            class LookUpSettings extends ObjectItem {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                updateFilter(parameter: any, report: Controls.ReportViewModel): void;
                constructor(model: any, dsHelperProvider?: any, serializer?: Analytics.Utils.IModelSerializer);
                filterString: any;
                _filterString: any;
            }
            class StaticListLookUpSettings extends LookUpSettings {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                afterDeserialization(model: any, serializer: any): void;
                updateFilter(parameter: any, report: Controls.ReportViewModel): void;
                constructor(model: any, dsHelperProvider?: any, serializer?: Analytics.Utils.IModelSerializer);
                lookUpValues: ko.ObservableArray<LookUpValue>;
            }
            class DynamicListLookUpSettings extends LookUpSettings {
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, dsHelperProvider?: any, serializer?: Analytics.Utils.IModelSerializer);
                dsHelperProvider: () => Internal.DataSourceHelper;
                dataSource: ko.Observable<ObjectStorageItem>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                getPath(propertyName: any): any;
            }
            class TableInfoCollectionItem extends Analytics.Elements.SerializableModel {
                constructor(model: any, dataSource: any, dsHelper: any, serializer?: Analytics.Utils.IModelSerializer);
                filterString: ko.Observable<Analytics.Widgets.FilterStringOptions>;
            }
            class UniversalDataSource extends ObjectItem {
                dispose(): void;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, dsHelperProvider?: any, serializer?: Analytics.Utils.IModelSerializer);
                parameters: ko.ObservableArray<ObjectStorageParameter>;
                tableInfoCollection: ko.ObservableArray<TableInfoCollectionItem>;
                spParameterInfoCollection: ko.ObservableArray<Analytics.Elements.SerializableModel>;
            }
            class ObjectsStorage extends Analytics.Utils.Disposable {
                constructor(objects: ko.ObservableArray<ObjectItem>, dsHelperProvider: any);
                findType(content: string): ObjectStorageItem;
                getType(type: string): ObjectStorageItem;
                addValue(): ObjectStorageItem;
                createStaticLookUpSetting(): StaticListLookUpSettings;
                createDynamicLookUpSetting(): DynamicListLookUpSettings;
                objects: ko.ObservableArray<ObjectItem>;
                dsHelperProvider: () => Internal.DataSourceHelper;
            }
        }
        module Internal {
            class CharacterCombHelper {
                static getAlignments(textAlignment: string): {
                    vertical: string;
                    horizontal: string;
                };
                static getLines(text: string, horizontal: number, multiline: boolean, wordwrap: boolean): any[];
                static getTextOffset(texts: string[], position: number, verticalAlign: string, horizontalAlign: string, vertical: number, horizontal: number): number;
                static setText(texts: string[], cells: Array<any>, getTextOffset: (texts: string[], position: number) => number): void;
                static distributionEmptySpace(emptySpace: number, vertical: boolean, textAlignment: string): number;
                static getHorizontalVerticalByText(multiline: boolean, wordwrap: boolean, text: string, horizontal: number, vertical: number): {
                    horizontal: number;
                    vertical: number;
                };
            }
        }
        module Controls {
            interface IReportControlMetadata extends Analytics.Elements.IElementMetadata {
                defaultBindingName?: string;
            }
            class ControlsFactory extends Analytics.Utils.ControlsFactory implements Analytics.Utils.IDisposable {
                fieldListProvider: ko.Observable | ko.Computed;
                dispose(): void;
                private _expressionWrapper;
                private _beforePrintPrintOnPage;
                private _beforePrint;
                private _registerCommonExpressions;
                private _registerExtensions;
                constructor(fieldListProvider?: ko.Observable | ko.Computed);
                registerControl(typeName: string, metadata: IReportControlMetadata): void;
                _createExpressionObject(typeName: any, expressions: any, path?: ko.Computed<string>, summaryRunning?: (name: string) => ko.Observable<boolean> | ko.Computed<boolean>): Internal.IExpressionObject;
                setExpressionBinding(controlType: string, propertyName: string, events: string[], group?: string, objectProperties?: string[]): void;
                setPropertyDescription(controlType: string, propertyName: string, events: string[], group?: string, objectProperties?: string[]): void;
                setLocalizationIdForExpression(propertyName: string, localizationId: string): void;
                hideExpressionBindings(type: any, ...propertyNames: any[]): void;
                hidePropertyDescriptions(type: any, ...propertyNames: any[]): void;
                inheritControl(parentType: string, extendedOptions: Analytics.Elements.IElementMetadata): any;
                createPopularBindingInfo(options: Metadata.ISerializationInfoWithBindings, isExpression?: boolean): Metadata.ISerializationInfoWithBindings;
            }
        }
        module Internal {
            class DataSourceHelper extends Analytics.Utils.Disposable {
                private _objects;
                availableDataSources: Analytics.Internal.IDataSourceInfo[];
                static defaultReportExtensionKey: string;
                dispose(): void;
                constructor(objects: ko.ObservableArray<Data.ObjectStorageItem>, dataSourceRefs: Utils.IDataSourceRefInfo[], availableDataSources: Analytics.Internal.IDataSourceInfo[]);
                getDataSourcePath(dataSource: any): string;
                _findDataSourceInfo(name: string, collection: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>): Analytics.Internal.IDataSourceInfo;
                _getDataSourceInfo(name: string): Analytics.Internal.IDataSourceInfo;
                _getDataSourceName(dataSource: Data.ObjectStorageItem): string;
                _addUsedDataSource(result: Analytics.Internal.IDataSourceInfo): void;
                _addDataSource(dataSource: Analytics.Internal.IDataSourceInfo, data: Data.ObjectItem): Analytics.Internal.IDataSourceInfo;
                addDataSource(dataSourceInfo: Analytics.Internal.IDataSourceInfo): Data.ObjectItem;
                removeDataSource(dataSourceInfo: Analytics.Internal.IDataSourceInfo): void;
                dataSourceValue(value: ko.Observable<Data.ObjectStorageItem>, undoEngine?: ko.Observable<Analytics.Utils.UndoEngine>): ko.PureComputed<string>;
                dataSourceDisplayExpr(dataSource: Analytics.Internal.IDataSourceInfo): any;
                usedDataSources: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>;
                allDataSources: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>;
                mergedDataSources(): Analytics.Internal.IDataSourceInfo[];
                findDataSourceInfo(dataSource: Data.ObjectItem): Analytics.Internal.IDataSourceInfo;
                findDataSourceInfoByID(id: string): Analytics.Internal.IDataSourceInfo;
                findDataSourceInfoByRef(ref: string): Analytics.Internal.IDataSourceInfo;
                findDataSourceInfoByName(name: string): Analytics.Internal.IDataSourceInfo;
                static _assignValueInTimeout: boolean;
            }
            class DesignControlsHelper extends Analytics.Internal.DesignControlsHelper {
                getNameProperty(model: any): any;
                protected _setName(value: any): void;
                constructor(target: Controls.ReportViewModel, selection: Analytics.Internal.SurfaceSelection);
            }
            class Locker {
                constructor();
                lock: (action: () => void) => void;
                isUpdate: boolean;
            }
            var reportStorageWebIsRegister: boolean;
            var limitation: boolean;
            function createReportDesigner(element: Element, data: Utils.IReportDesignerInitializationData, callbacks: {
                designer?: Utils.IReportDesignerCustomizationHandler;
                preview?: DevExpress.Reporting.Viewer.Utils.IPreviewCustomizationHandler;
            }, localizationSettings?: Analytics.Internal.ILocalizationSettings, knownEnums?: Array<DevExpress.Reporting.IEnumType>, designerHandlerUri?: string, previewHandlerUri?: string, rtl?: boolean, applyBindings?: boolean): JQueryDeferred<any>;
            function createReportDesignerFromModel(model: Utils.IReportDesignerInitializationModel, element: Element, callbacks?: {
                designer?: Utils.IReportDesignerCustomizationHandler;
                preview?: DevExpress.Reporting.Viewer.Utils.IPreviewCustomizationHandler;
            }, applyBindings?: boolean): JQueryDeferred<any>;
            class WizardRunner extends Analytics.Utils.Disposable {
                private _menuOptions;
                dispose(): void;
                private _currentWizard;
                private _wizards;
                constructor(_menuOptions: {
                    visible: ko.Subscribable<boolean>;
                    collapsed: ko.Subscribable<boolean>;
                });
                registerWizard(wizardType: Wizard.WizardRunType, start: () => void, close: () => void): void;
                runWizard(wizardType: Wizard.WizardRunType): void;
            }
            function patchRequest(request: Analytics.Utils.IPathRequest, dataSources: Analytics.Internal.IDataSourceInfo[], state: any): void;
            class FieldListDataSourcesHelper implements Analytics.Utils.IDisposable {
                private _fieldListCache;
                private _dataSourceSubscriptions;
                private _usedDataSourceSubscription;
                private _renameDataSourceStrategy;
                private _cacheIsClearNotificicator;
                dataSourceHelper: ko.Observable<DataSourceHelper>;
                fieldListDataSources: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>;
                dispose(): void;
                private _clearDataSourceCache;
                private _subscribeDataSource;
                private _updateFieldListDataSources;
                constructor();
                wrapFieldsCallback(fieldsCallback: (IPathRequest: any) => JQueryPromise<Analytics.Utils.IDataMemberInfo[]>, state: () => {}): (request: Analytics.Utils.IPathRequest) => JQueryPromise<Analytics.Utils.IDataMemberInfo[]>;
                _subscribeDataSources(usedDataSources: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>, model: any): void;
                updateDataSources(dsHelper: DataSourceHelper, model: any, parameters?: any): void;
            }
            class FieldListDragDropHelper {
                private _dataBindingMode;
                private _size?;
                constructor(_dataBindingMode: string, _size?: Analytics.Elements.Size);
                private _createTable;
                private _getItemsFromList;
                private _getFirstLevelItems;
                createTableFromListSource(treeListItem: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel, parent: any): JQueryPromise<Analytics.Elements.IElementViewModel>;
                createTableFromItems(treeListItems: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel[], parent: any): JQueryPromise<Analytics.Elements.IElementViewModel>;
            }
            class ReportDesignerControlsHelper extends Analytics.Utils.Disposable implements Analytics.Internal.IDesignControlsHelper {
                constructor(helper: ko.Computed<DesignControlsHelper>);
                getControls: (target: any) => ko.ObservableArray<Analytics.Internal.IDisplayedObject>;
                allControls: any;
            }
            class TextElementSizeHelper {
                private _spaceSymbol;
                private _$createElement;
                $createTextElement(text: string, options: Object): JQuery;
                $createSpaceElement(options: Object): JQuery;
                getTextContainerSize(text: any, options: any, increaseHeight?: number): {
                    width: number;
                    height: number;
                };
            }
        }
        module Bands {
            module Internal {
                function sortBands(band1: any, band2: any): number;
                function setMarkerWidth(bandHolder: IBandsHolder, levelCount: any, currentLevel?: number): void;
                function getLevelCount(bandHolder: IBandsHolder): number;
            }
        }
        module Internal {
            module HtmlMarkUp {
                interface IDiplayNameParameters {
                    text: string;
                    wordWrap?: boolean;
                    fontSize?: number;
                    fontUnit?: string;
                }
                interface IInheritValues {
                    fontSize?: number;
                    backcolor?: boolean;
                }
                interface ITag {
                    node: Element;
                    element: HTMLElement;
                    createElement: () => any;
                    appendTo: (el: HTMLElement) => void;
                    hasChildNodes: boolean;
                    setProperties: (parameters?: IDiplayNameParameters, inheritValues?: IInheritValues) => any;
                    value?: any;
                    inheritValues: IInheritValues;
                }
                class ValueConverter {
                    private _displayNameParameters;
                    static ValueAttrName: string;
                    private _regExp;
                    private _createTag;
                    private _parceToXml;
                    private _checkValidTag;
                    private _createTree;
                    constructor(_displayNameParameters: IDiplayNameParameters);
                    appendTo(element: HTMLElement): void;
                }
            }
            class CalculatedFieldsSource extends Analytics.Utils.Disposable implements Analytics.Internal.IActionsProvider, Analytics.Internal.IItemsExtender {
                dispose(): void;
                private _calculatedFieldsInfo;
                private _ordinaryFieldsInfo;
                private _calculatedFields;
                private _dataSourceHelper;
                private _reportDataSource;
                private _fieldsDataMembersInfo;
                private _fieldsCallback;
                private _getDataMembersInfoByPath;
                private _subscribeFieldProperties;
                private _getFieldPathRequest;
                private _updateFieldPathRequest;
                private _initializeCalculatedField;
                private _generateNewFieldName;
                constructor(calculatedFields: ko.ObservableArray<Data.CalculatedField>, reportDataSource: ko.Observable<Data.ObjectStorageItem>, dataSourceHelper: DataSourceHelper);
                createCalculatedField(dataMember: string): Data.CalculatedField;
                addAction: Analytics.Utils.IAction;
                removeAction: Analytics.Utils.IAction;
                getActions(context: any): Analytics.Utils.IAction[];
                beforeItemsFilled(request: DevExpress.Analytics.Utils.IPathRequest, items: DevExpress.Analytics.Utils.IDataMemberInfo[]): boolean;
                afterItemsFilled(request: DevExpress.Analytics.Utils.IPathRequest, items: DevExpress.Analytics.Utils.IDataMemberInfo[]): void;
                addCalculatedField: (fullPath: string) => Data.CalculatedField;
                removeCalculatedField: (fullPath: string) => void;
            }
            class CoordinateGridViewModel extends Analytics.Utils.Disposable {
                _initGrid(length: number, gridSize: number, gridLines: ko.ObservableArray<any>, flip?: boolean): void;
                constructor(options: {
                    height: ko.Observable<number> | ko.Computed<number>;
                    width: ko.Observable<number> | ko.Computed<number>;
                    snapGridSize: ko.Observable<number> | ko.Computed<number>;
                    zoom: ko.Observable<number> | ko.Computed<number>;
                    measureUnit: ko.Observable<string> | ko.Computed<string>;
                    flip?: ko.Observable<boolean> | ko.Computed<boolean>;
                });
                width: ko.Observable<number>;
                height: ko.Observable<number>;
                verticalGridLines: ko.ObservableArray<any>;
                horizontalGridLines: ko.ObservableArray<any>;
                majorVerticalGridLines: ko.ObservableArray<any>;
                majorHorizontalGridLines: ko.ObservableArray<any>;
                dispose(): void;
            }
            class CustomMergingEngine {
                private _customMergeForFormatString;
                customMerge(propertyName: string, controls: {}[], undoEngine: ko.Observable<Analytics.Utils.UndoEngine>): {
                    result: ko.ObservableArray<any>;
                    subscriptions: any[];
                };
            }
            enum DataBindingMode {
                Bindings = "Bindings",
                Expressions = "Expressions",
                ExpressionsAdvanced = "ExpressionsAdvanced"
            }
            function addDataSourceToReport(dataSourceHelper: DataSourceHelper, report: Controls.ReportViewModel, undoEngine: Analytics.Utils.UndoEngine, itemsProvider: Analytics.Utils.IItemsProvider, dataSource: Analytics.Internal.IDataSourceInfo, forceAssigning?: boolean): void;
            function includeNonListItem(dataMembers: Analytics.Utils.IDataMemberInfo[]): boolean;
            function removeDataSourceFromReport(dataSourceHelper: DataSourceHelper, reportDataSource: ko.Observable<Data.ObjectItem> | ko.Computed<Data.ObjectItem>, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, dataSource: Analytics.Internal.IDataSourceInfo): void;
            function getDataSourceDataMember(control: any): {
                dataSource: Data.ObjectStorageItem;
                dataMember: string;
            };
            var reportCopyPasteStrategy: (componentAdded?: any) => Analytics.Internal.ICopyPasteStrategy;
            class DataSourceItemsExtender implements Analytics.Internal.IItemsExtender {
                private _dataSources;
                constructor(dataSources: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>);
                beforeItemsFilled(request: DevExpress.Analytics.Utils.IPathRequest, items: DevExpress.Analytics.Utils.IDataMemberInfo[]): boolean;
                afterItemsFilled(request: DevExpress.Analytics.Utils.IPathRequest, items: DevExpress.Analytics.Utils.IDataMemberInfo[]): void;
            }
            class DisplayNameProvider implements Analytics.Utils.IDisplayNameProvider {
                private _fieldsProvider;
                private _dataSourceHelper;
                private _rootDS;
                private _requests;
                private _getRequest;
                private _getDisplayNameRequest;
                private _createRequestInfo;
                private _getFieldDisplayName;
                private _getDisplayName;
                private _getRealName;
                private _getRealNameRequest;
                constructor(_fieldsProvider: Analytics.Utils.IItemsProvider, _dataSourceHelper: DataSourceHelper, _rootDS: ko.Observable<Data.ObjectStorageItem>);
                getDisplayName(dataSource: Data.ObjectStorageItem, dataMember: string, dataMemberOffset?: string, includeDataSourceName?: boolean): JQueryPromise<string>;
                getDisplayNameByPath(path: string, dataMember: string): JQueryPromise<string>;
                getRealName(path: string, dataMember: string): JQueryPromise<string>;
                private _getByPath;
                dispose(): void;
            }
            var StringId: {
                Copy: string;
                NewViaWizard: string;
                Open: string;
                Save: string;
                SaveAs: string;
                MdiReportChanged: string;
            };
            class FieldListController implements DevExpress.Analytics.Widgets.Internal.ITreeListController {
                private _actionProviders;
                private _fieldListActionWrapper;
                private _customizeFieldListActions;
                private _selectedItems;
                dispose(): void;
                constructor(actionProviders?: Analytics.Internal.IActionsProvider[], fieldListActionWrapper?: (actions: Analytics.Utils.IAction[]) => void, dragDropHandler?: Analytics.Internal.DragDropHandler, customizeFieldListActions?: (item: Analytics.Utils.IDataMemberInfo, actions: Analytics.Utils.IAction[]) => void);
                itemsFilter(item: DevExpress.Analytics.Utils.IDataMemberInfo): boolean;
                static isList(item: DevExpress.Analytics.Utils.IDataMemberInfo): boolean;
                hasItems: typeof FieldListController.isList;
                select(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): void;
                canSelect(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): boolean;
                getActions(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): Analytics.Utils.IAction[];
                canMultiSelect(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): any;
                multiSelect(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel, isShiftPressed?: boolean, isCtrlPressed?: boolean): void;
                isDraggable(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): boolean;
                dragDropHandler: Analytics.Internal.DragDropHandler;
                selectedItem: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel;
                selectedItems(): Analytics.Widgets.Internal.TreeListItemViewModel[];
            }
            class ParametersViewModel extends Analytics.Utils.Disposable implements Analytics.Internal.IActionsProvider, Analytics.Internal.IItemsExtender {
                constructor(report: Controls.ReportViewModel);
                parameters: ko.ObservableArray<Data.Parameter>;
                addAction: {
                    clickAction: (item: any) => any;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: string;
                    displayText: () => any;
                };
                removeAction: {
                    clickAction: (item: any) => void;
                    imageClassName: string;
                    imageTemplateName: string;
                    text: string;
                    displayText: () => any;
                };
                getActions(context: any): Analytics.Utils.IAction[];
                add: any;
                remove: any;
                createParameter: () => Data.Parameter;
                beforeItemsFilled(request: Analytics.Utils.IPathRequest, items: DevExpress.Analytics.Utils.IDataMemberInfo[]): boolean;
                afterItemsFilled(request: DevExpress.Analytics.Utils.IPathRequest, items: DevExpress.Analytics.Utils.IDataMemberInfo[]): void;
            }
            interface IComponentNameValidator {
                validateName: (nameCandidate: string) => boolean;
                validateUnique: (nameCandidate: string, currentName: string) => boolean;
            }
            interface IRenameComponentStrategy extends IComponentNameValidator {
                tryRename: (nameCandidate: string, currentItemData: Data.ObjectItem) => boolean;
            }
            class RenameDataSourceStrategy implements IRenameComponentStrategy {
                dsHelper: ko.Observable<DataSourceHelper>;
                private _afterRenameCallBack?;
                private _rename;
                constructor(dsHelper: ko.Observable<DataSourceHelper>, _afterRenameCallBack?: () => void);
                validateName(nameCandidate: string): boolean;
                validateUnique(nameCandidate: any, currentName: string): boolean;
                tryRename(nameCandidate: string, currentItemData: Data.ObjectItem): boolean;
            }
            interface IRulesDictionaryItem {
                condition: string;
                dataMember: string;
                dataSource: Data.ObjectStorageItem;
                formatting: any;
            }
            class ReportConverter {
                private _controlsHelper;
                private _undoEngine;
                private _dataBindingMode;
                private convertChoiceEnum;
                private _formattingMapper;
                private _mapRulesProperties;
                private _expressionsToControlMap;
                private _model;
                private _lastChoice;
                private _defaultFormatting;
                private _notShowAgain;
                private _detailLink;
                popupOptions: {
                    visible: ko.Observable<boolean>;
                    title: any;
                    confirmMessage: string;
                    linkText: any;
                    linkUrl: string;
                    container: (element: HTMLElement) => JQuery;
                    buttons: ({
                        toolbar: string;
                        location: string;
                        widget: string;
                        options: {
                            text: any;
                            onClick: () => void;
                            value?: undefined;
                        };
                    } | {
                        toolbar: string;
                        location: string;
                        widget: string;
                        options: {
                            value: ko.Observable<boolean>;
                            text: any;
                            onClick?: undefined;
                        };
                    })[];
                };
                constructor(_controlsHelper: any, _undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, _dataBindingMode?: string);
                private _hasBindings;
                private _hasFormattingRules;
                convert(model: Controls.ReportViewModel, convertBindingsToExpressions?: string): void;
                private _generateStyleName;
                private _createBindingExpression;
                private _tryToGenerateBindingExpressions;
                private _resetDataBindings;
                private _mapPaddingObj;
                private _mapFontObj;
                private _splitFontPropertyValue;
                private _splitPaddingPropertyValue;
                private _patchRuleCondition;
                private _tryToGenerateFormattingRulesExpressions;
                private _getControlDataSourceDataMember;
                private _generateFormattingRulesDictionary;
                private _createRuleExpression;
                private _canConvertReport;
                private _applyChanges;
                private _cancel;
            }
            class ReportExplorerModel extends Analytics.Utils.Disposable {
                static getPathByMember(model: any): any;
                private _createActionsForOneElement;
                private _createActionsForArray;
                private _getPathNonControl;
                constructor(reportModel: ko.Observable<Controls.ReportViewModel>, editableObject: any, clickHandler: any, dragDropHandler: ReportExplorerDragDropHandler, selection: Analytics.Internal.ISelectionProvider);
                itemsProvider: Analytics.Internal.ObjectExplorerProvider;
                treeListController: Analytics.Internal.ObjectStructureTreeListController;
            }
            type IReportItemsProviderRootItems = {
                [key: string]: (path: string, controlsHelper: Analytics.Internal.DesignControlsHelper) => Analytics.Utils.IDataMemberInfo[];
            };
            class ReportItemsProvider extends Analytics.Utils.Disposable implements Analytics.Utils.IItemsProvider {
                private _rootItems;
                private _getControlByName;
                private _getProperties;
                private _tryGenerateGetItemsFunc;
                getReportElementsByPath(controlsHelper: any, path: string[]): Analytics.Utils.IDataMemberInfo[];
                getItems: (path: DevExpress.Analytics.Utils.IPathRequest, rootItems?: IReportItemsProviderRootItems) => JQueryPromise<Analytics.Utils.IDataMemberInfo[]>;
                getItemByPath: (path: Analytics.Utils.IPathRequest, rootItems?: IReportItemsProviderRootItems) => JQueryPromise<Analytics.Utils.IDataMemberInfo>;
                constructor(controlsHelper: Analytics.Internal.DesignControlsHelper, fieldListProvider: Analytics.Utils.IItemsProvider);
                _getItemByPath(pathRequest: Analytics.Utils.IPathRequest, rootItems: IReportItemsProviderRootItems, askParents: boolean): JQueryPromise<Analytics.Utils.IDataMemberInfo>;
            }
        }
        module Wizard {
            module Legacy {
                class RequestReportModel {
                    AdjustFieldWidth: boolean;
                    Columns: Array<string>;
                    ColumnInfo: {
                        Name: string;
                        DisplayName: string;
                        TypeSpecifics: number;
                    }[];
                    CustomLabelInformation: Internal.ICustomLabelInformation;
                    DataMemberName: {
                        "DisplayName": string;
                        "Name": string;
                        "DataMemberType": number;
                    };
                    DataSourceName: string;
                    GroupingLevels: string[][];
                    IgnoreNullValuesForSummary: boolean;
                    LabelProductId: number;
                    LabelProductDetailId: number;
                    Layout: ReportLayout;
                    Portrait: boolean;
                    ReportStyleId: ReportStyle;
                    ReportTitle: string;
                    ReportType: number;
                    SummaryOptions: {
                        ColumnName: string;
                        Flags: number;
                    }[];
                    UseMasterDetailBuilder: boolean;
                    PaperKind: Utils.PaperKind;
                    PaperSize: {
                        width: number;
                        height: number;
                    };
                    Margins: {
                        Top: number;
                        Right: number;
                        Bottom: number;
                        Left: number;
                    };
                    Unit: GraphicsUnit;
                    constructor(reportWizardModel: ReportWizardModel);
                }
                class RequestXtraReportModel extends RequestReportModel {
                    private _masterRelationMap;
                    private _collectionByPath;
                    DataSourceName: string;
                    MasterDetailInfo: any;
                    MasterDetailGroupsInfo: {
                        [key: string]: any;
                    };
                    MasterDetailSummariesInfo: {
                        [key: string]: {
                            ColumnName: string;
                            Flags: number;
                        }[];
                    };
                    constructor(reportWizardModel: ReportWizardModel);
                }
            }
        }
        module Internal {
            class RulerViewModel extends Analytics.Utils.Disposable {
                _initGrid(length: number, gridSize: any, gridLines: any, flip?: boolean): void;
                constructor(options: {
                    length: () => number;
                    units: ko.Observable<string> | ko.Computed<string>;
                    zoom: ko.Observable<number> | ko.Computed<number>;
                    direction?: string;
                    flip?: any;
                    disable?: {
                        start: number;
                        width: number;
                    };
                });
                height: ko.Observable<number>;
                width: ko.Observable<number>;
                gridLines: ko.ObservableArray<any>;
                majorGridLines: ko.ObservableArray<any>;
                disable: any;
                defaultGridLinesCoordinate: ko.Observable<any>;
            }
            function findFirstParentWithPropertyName(control: any, propertyName: any): any;
            function addVariablesToExpressionEditor(categories: any, customizeItems?: (items: any[]) => any[]): void;
            function createIDataMemberInfoByName(name: string, specifics?: string): {
                displayName: string;
                name: string;
                specifics: string;
                isList: boolean;
            };
            function recalculateUnit(value: any, dpi: number): number;
            var PromptBoolean: {
                "False": string;
                "True": string;
                "Prompt": string;
            };
            function correctModel(model: any): any;
            function createObjectFromInfo(control: Controls.XRReportElementViewModel, serializationsInfo: Analytics.Utils.ISerializationInfoArray): any;
            function createReportViewModel(newReportInfo: {
                reportModel: string;
                dataSourceRefs: Utils.IDataSourceRefInfo[];
            }, oldReport: Controls.ReportViewModel): Controls.ReportViewModel;
            function updateDataSourceRefs(report: Controls.ReportViewModel, dataSourceRefs: any): void;
            function isNotParameter(control: any): boolean;
            function isControl(control: any): boolean;
            interface IReportWatermark extends Analytics.Internal.IStyleContainer {
                text: ko.Observable<string> | ko.Computed<string>;
                textDirection: ko.Observable<string> | ko.Computed<string>;
                font: ko.Observable<string> | ko.Computed<string>;
                foreColor: ko.Observable<string> | ko.Computed<string>;
                textTransparency: ko.Observable<string> | ko.Computed<string>;
                image: ko.Observable<string> | ko.Computed<string>;
                imageTransparency: ko.Observable<number> | ko.Computed<number>;
                imageTiling: ko.Observable<boolean> | ko.Computed<boolean>;
                imageAlignment: ko.Observable<string> | ko.Computed<string>;
                imageViewMode: ko.Observable<string> | ko.Computed<string>;
                pageRange: ko.Observable<string> | ko.Computed<string>;
                showBehind: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            interface WatermarkBindingOptions {
                band: Bands.BandSurface;
                reportSurface: Controls.ReportSurface;
                forLeftMargin: boolean;
                image: string;
                transparency: number;
                viewMode: string;
                align: string;
                tiling: boolean;
            }
            function selectTreeListItem(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel, event: JQueryEventObject): void;
            function getExpressionPath(container: any, path: any): string;
            function getUsefulReportWidth(surface?: Controls.ReportSurface): Analytics.Elements.ISize;
            function createPictureBox(container: Analytics.Elements.ElementViewModel, bindingPath: string, dataBindingMode: string): any;
            var memberControlsMap: {
                "Array": {
                    drop: (treeListItem: Analytics.Widgets.Internal.TreeListItemViewModel, dropTargetControl: Analytics.Elements.ElementViewModel, dataBindingMode: any) => any;
                    size: (surface?: Analytics.Elements.ISurfaceContext) => Analytics.Elements.ISize;
                };
                "Bool": {
                    drop: (treeListItem: Analytics.Widgets.Internal.TreeListItemViewModel, dropTargetControl: Analytics.Elements.ElementViewModel, dataBindingMode: any) => any;
                    size: (surface?: Analytics.Elements.ISurfaceContext) => Analytics.Elements.ISize;
                };
                "List": {
                    drop: (treeListItem: Analytics.Widgets.Internal.TreeListItemViewModel, dropTargetControl: Analytics.Elements.ElementViewModel, dataBindingMode: any, size?: Analytics.Elements.Size) => JQueryPromise<Analytics.Elements.IElementViewModel>;
                    size: typeof getUsefulReportWidth;
                    adjustDropTarget: (dropTarget: Analytics.Internal.ISelectionTarget) => any;
                };
                "MultiList": {
                    drop: (treeListItem: Analytics.Widgets.Internal.TreeListItemViewModel, dropTargetControl: Analytics.Elements.ElementViewModel, dataBindingMode: any, size?: Analytics.Elements.Size) => JQueryPromise<Analytics.Elements.IElementViewModel>;
                    size: typeof getUsefulReportWidth;
                    adjustDropTarget: (dropTarget: Analytics.Internal.ISelectionTarget) => any;
                };
                "Default": {
                    drop: (treeListItem: Analytics.Widgets.Internal.TreeListItemViewModel, dropTargetControl: Analytics.Elements.ElementViewModel, dataBindingMode: any) => any;
                    size: (surface?: Analytics.Elements.ISurfaceContext) => Analytics.Elements.ISize;
                };
            };
            function createSimpleControl(controlType: string, dropTargetControl: Analytics.Elements.ElementViewModel): any;
            function assignBinding(control: any, container: any, bindingName: string, item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel, dataBindingMode: any): any;
            function getBandIfItDoesNotContainTOC(reportModel: Controls.ReportViewModel, bandType: string): Bands.BandViewModel;
            function isList(data: Analytics.Utils.IDataMemberInfo): boolean;
            function dragDropComponentAdded(model: any, parent: any): void;
            class SelectionDragDropHandler extends Analytics.Internal.SelectionDragDropHandler {
                getLocation(adjustedTarget: any, item: any): Analytics.Elements.IArea;
            }
            class FieldListDragDropHandler extends Analytics.Internal.DragDropHandler {
                private _undoEngine;
                private _dataSources;
                private _getKey;
                private _setDragHelperContent;
                private _getDropTarget;
                private _needToChangeHelperContent;
                private _updateInnerControlSize;
                private _addControl;
                private _isDefaultBindingAssigned;
                constructor(surface: ko.Observable<Controls.ReportSurface> | ko.Computed<Controls.ReportSurface>, selection: Analytics.Internal.SurfaceSelection, _undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, snapHelper: Analytics.Internal.SnapLinesHelper, dragHelperContent: Analytics.Internal.DragHelperContent, _dataSources: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>, onComponentAdded?: any);
                drag(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
                doStopDrag(ui: any, draggable: any): void;
                onComponentAdded: (e: Utils.IComponentAddedEventArgs) => void;
                dataBindingMode: ko.Computed<string>;
            }
            class ReportControlsDragDropHelper {
                private _undoEngine?;
                private _draggable;
                private _draggableModel;
                private _draggableParent;
                private _serializer;
                private _target;
                private _targetElement;
                private _getElementViewModel;
                private _canReorder;
                private _shiftChildrenCount;
                private _targetIsClosestOfDraggable;
                private _canDrop;
                private _removeClass;
                private _insertTableChilds;
                constructor(draggable: Analytics.Widgets.Internal.TreeListItemViewModel, _undoEngine?: Analytics.Utils.UndoEngine);
                setNewDropTarget(elementModel: any, element: HTMLElement): void;
                doStopDrag(): Analytics.Internal.ISelectionTarget;
            }
            class ReportSnapLinesCollector extends Analytics.Internal.SnapLinesCollector {
                private _rtl;
                _getCollection(parent: any): {
                    rect: ko.Observable<Analytics.Elements.IArea>;
                }[];
                private _enumerateBandCollection;
                private _processBandRtl;
                _enumerateCollection(parent: any, parentAbsoluteProsition: {
                    top: number;
                    left: number;
                }, callback: (item: any, itemAbsoluteRect: {
                    left: number;
                    right: number;
                    top: number;
                    bottom: number;
                }) => void): void;
                constructor(_rtl: ko.Observable<boolean> | ko.Computed<boolean>);
            }
            class ReportExplorerDragDropHandler extends Analytics.Internal.DragDropHandler {
                private undoEngine;
                private _lastList;
                private _timeout;
                private _isStyle;
                private _isFormatingRule;
                private _isReportControl;
                constructor(surface: ko.Observable<Analytics.Elements.ISurfaceContext> | ko.Computed<Analytics.Elements.ISurfaceContext>, selection: Analytics.Internal.SurfaceSelection, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, dragHelperContent: Analytics.Internal.DragHelperContent);
                startDrag(draggable: any): void;
                drag(event: JQueryEventObject, ui: JQueryUI.DraggableEventUIParams): void;
                doStopDrag(ui: any, draggable: any, event: JQueryEventObject): void;
                reportControlsDragDropHelper: ReportControlsDragDropHelper;
            }
            class ReportToolboxDragDropHandler extends Analytics.Internal.ToolboxDragDropHandler {
                dispose(): void;
                surface: ko.Observable<Controls.ReportSurface> | ko.Computed<Controls.ReportSurface>;
                constructor(surface: ko.Observable<Controls.ReportSurface> | ko.Computed<Controls.ReportSurface>, selection: Analytics.Internal.SurfaceSelection, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, snapHelper: Analytics.Internal.SnapLinesHelper, dragHelperContent: Analytics.Internal.DragHelperContent, controlsFactory: Analytics.Utils.ControlsFactory, onComponentAdded?: any);
                helper(draggable: any): void;
                private _processProperty;
                doStopDrag(ui: any, draggable: any): void;
                addControl(control: Analytics.Elements.IElementViewModel, dropTargetSurface: any, size: any): void;
                onComponentAdded: (e: Utils.IComponentAddedEventArgs) => void;
            }
            var eventArgsTypes: {
                [key: string]: string;
            };
            class dxEventDropDownEditor extends dxSelectBox {
                _secondAction: any;
                _$ellipsisButton: any;
                _koContext: any;
                _getDefaultOptions(): any;
                _init(): void;
                _initSecondAction(): void;
                _render(): void;
                _renderDropDownButton(): void;
                _createEllipsisButton(): JQuery;
                _attachEllipsisButtonClickHandler(): void;
                _optionChanged(args: any): void;
            }
            class LanguageHelper {
                private _report;
                getLanguageMode(): "ace/mode/csharp" | "ace/mode/vbscript" | "ace/mode/text";
                createNewHandler(eventName: string, eventArgsType: string): string;
                getFunctionNamesFromScript(scripts: string): any[];
                constructor(report: ko.Observable<Controls.ReportViewModel>);
                createCompleters(editor: any, bindingContext: any, viewModel: any): Array<{
                    getCompletions: any;
                }>;
            }
            class ReportCompleter {
                __getCompletions(editor: any, session: any, pos: any, prefix: any, callback: any): void;
                constructor(report: ko.Observable<Controls.ReportViewModel>, editorInstance: any, guid: any);
                getCompletions(editor: any, session: any, pos: any, prefix: any, callback: any): void;
                completions: any[];
                oldPrefix: string;
                report: ko.Observable<Controls.ReportViewModel>;
                editorInstance: any;
                guid: ko.Observable<string> | ko.Computed<string>;
            }
            interface ICursorPosition {
                row: number;
                column: number;
            }
            interface IAceEditor {
                setValue: (text: string) => void;
                getValue: () => string;
                getSession: () => any;
                getSelection: () => any;
                getCopyText: () => string;
                getCursorPosition: () => ICursorPosition;
                onPaste: (text: string) => void;
                execCommand: (cmd: string) => void;
                undo: (select: boolean) => void;
                redo: (select: boolean) => void;
                on: (event: string, handler: any) => void;
                resize: () => void;
                find: (needle: string, options: any, animate: boolean) => void;
                findNext: () => void;
                findPrevious: () => void;
                focus: () => any;
                guid: string;
            }
            class ScriptsEditor extends Analytics.Utils.Disposable {
                private _selectionNotEmpty;
                private _canUndo;
                private _canRedo;
                private _cursorPosition;
                private _changeSelection;
                private _updateEditorState;
                private _initializeToolbar;
                private _getValidIndex;
                private _setScriptsText;
                private _getFunctionName;
                private _getEventByFunction;
                static generateFunctionName(control: Controls.XRReportElementViewModel, eventName: string, functionName?: string, allFunctionNames?: any[]): string;
                static getEventArgsType(eventName: string): string;
                static _createReportDummy(report: any): any;
                initialize(): void;
                constructor(report: ko.Observable<Controls.ReportViewModel>, allControls: ko.ObservableArray<Controls.XRReportElementViewModel>);
                readonly allFunctionNames: any[];
                guid: ko.Observable<any>;
                ensureEvent: (eventName: string, functionName?: string, model?: any) => void;
                private _ensureFunction;
                selectionChanged: (editor: IAceEditor) => void;
                report: ko.Observable<Controls.ReportViewModel>;
                scriptsText: ko.Observable<string> | ko.Computed<string>;
                editorContainer: ko.Observable<IAceEditor>;
                editorVisible: ko.Observable<boolean>;
                toolbarItems: any[];
                controls: ko.ObservableArray<Controls.XRReportElementViewModel>;
                selectedControl: ko.Observable<Controls.XRReportElementViewModel>;
                events: ko.Observable<string[]>;
                selectedEvent: ko.Observable<string>;
                languageHelper: LanguageHelper;
                validateDisabled: ko.Observable<boolean>;
                aceOptions: {
                    enableBasicAutocompletion: boolean;
                    enableSnippets: boolean;
                    enableLiveAutocompletion: boolean;
                    showPrintMargin: boolean;
                };
            }
            class FormatStringService {
                static saveCustomPattern(typeString: string, format: string): any;
                static removeCustomPattern(typeString: string, format: string): any;
                static updatePreview(value: string, typeString: string, format: string): any;
                static actions: Analytics.Widgets.IFormatStringEditorActions;
            }
            class ReportDataSourceService {
                static fieldListCallback(request: Analytics.Utils.IPathRequest): JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
                static getCustomQueriesPreset(dataSource: Analytics.Data.SqlDataSource): JQueryPromise<Analytics.Data.TableQuery[]>;
                static sqlDataSourceFromBase64(base64: string): JQueryPromise<Analytics.Data.SqlDataSource>;
                static getSqlDataSourceBase64(dataSource: Analytics.Data.SqlDataSource): JQueryPromise<string>;
                static getJsonDataSourceBase64(dataSource: Analytics.Data.JsonDataSource): JQueryPromise<{
                    base64: string;
                    schema: string;
                }>;
                static jsonDataSourceFromBase64(base64: string): JQueryPromise<{
                    jsonDataSourceJSON: string;
                }>;
            }
            class ReportPreviewService {
                static initializePreview(report: Controls.ReportViewModel): any;
            }
            class ReportRenderingService {
                static getChartImage(surface: Controls.XRChartSurface): any;
                static getShapeImage(surface: Controls.XRShapeControlSurface): string;
                static getRichImage(surface: Controls.XRRichSurface, propertyName: any): any;
            }
            class ReportScriptService {
                static validateScripts(report: Controls.ReportViewModel): any;
                static getCompletions(editor: any, session: any, pos: any, prefix: any, callback: any, report: any, editorInstance: any, guid: string): any;
                static setCodeDom(key: string, reportLayout: string): any;
            }
            class ReportStorageWeb {
                static getErrorMessageHandler(defaultErrorMessage?: string): (messageFromDefaultHandler: string, jqXHR: JQueryXHR, textStatus: string) => void;
                static getReportByUrl(url: string): JQueryPromise<Controls.ReportViewModel>;
                static getData(url: string): any;
                static setData(layout: string, url: string): any;
                static setNewData(layout: string, url: string): JQueryPromise<any>;
                static getUrls(subreports?: any): any;
            }
            class ReportWizardService {
                static createWizardRequest(reportWizardModel: Wizard.Legacy.ReportWizardModel, state: any, customizeWizardModelAction: (wizardModel: any) => void, oldReportJSON?: string): string;
                static createNewWizardRequest(reportWizardState: Wizard.IReportWizardState, requestType: any, state: any, customizeWizardModelAction: (wizardModel: any) => void, oldReportJSON?: any): string;
                static generateReportFromWizardModel(reportWizardModel: Wizard.Legacy.ReportWizardModel, state: any, customizeWizardModelAction: (wizardModel: any) => void, oldReportJSON?: string): any;
                static generateReportFromWizardState(reportWizardState: Wizard.IReportWizardState, requestType: any, state: any, customizeWizardModelAction: (wizardModel: any) => void, oldReportJSON?: string): any;
                static createDataSource(reportWizardModel: Wizard.Legacy.ReportWizardModel, state: any): any;
                static getLabelReportWizardData(): any;
                static createNewJsonDataSource(state: Analytics.Wizard.IJsonDataSourceWizardState, createJsonCallback: (dataSource: Analytics.Data.JsonDataSource) => JQueryPromise<IDataSourceInfo>): JQueryPromise<string>;
            }
        }
        module Wizard {
            class _ReportWizardOptions extends Analytics.Wizard._DataSourceWizardOptionsBase<Internal.IReportWizardCallbacks> {
                callbacks: Internal.IReportWizardCallbacks;
                wizardSettings: Utils.IReportWizardSettings;
                dataSources: ko.PureComputed<Analytics.Internal.IDataSourceInfo[]>;
                hideDataMemberSubItems: boolean;
            }
        }
        module Internal {
            interface IReportWizardCallbacks extends Analytics.Wizard.Internal.IMultiQueryDataSourceWizardCallbacks {
                fieldListsCallback: IReportWizardFieldsCallback;
                createSqlDataSourceInfo: (dataSource: Analytics.Data.SqlDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>;
                createJsonDataSourceInfo: (dataSource: Analytics.Data.JsonDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>;
            }
            interface IReportWizardFieldsCallback {
                (request: Analytics.Utils.IPathRequest, dataSource: Analytics.Internal.IDataSourceInfo): JQueryPromise<DevExpress.Analytics.Utils.IDataMemberInfo[]>;
            }
            var _masterDetailWizardHeight: string;
            var _masterDetailWizardWidth: string;
            var _masterDetailScrollViewHeight: string;
            function overrideFullscreenDataSourceWizardPageMetadata(factory: Analytics.Wizard.PageFactory, pageId: string, create: () => Analytics.Wizard.FullscreenWizardPage): void;
        }
        module Wizard {
            interface ILabelProduct {
                id: number;
                name: string;
            }
            interface IPaperKind {
                id: number;
                enumId: number;
                name: string;
                width: number;
                height: number;
                isRollPaper: boolean;
                unit: GraphicsUnit;
            }
            interface ILabelDetails {
                id: number;
                productId: number;
                paperKindId: number;
                name: string;
                width: number;
                height: number;
                hPitch: number;
                vPitch: number;
                topMargin: number;
                leftMargin: number;
                rightMargin: number;
                bottomMargin: number;
                unit: GraphicsUnit;
            }
        }
        module Internal {
            var CONVERSION_COEEFICIENT: number;
        }
        module Wizard {
            class ColorScheme {
                _isCustom: boolean;
                constructor(name: string, localizationId: string, baseColor: string);
                name: string;
                localizationId: string;
                baseColor: string;
                color: string | ko.Observable<string> | ko.Computed<string>;
                displayName: string;
                selected: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            class CustomColorScheme extends ColorScheme {
                applyColor(): void;
                resetColor(): void;
                constructor(name: string, localizationId: string, baseColor: string);
                editorColor: ko.Observable<string> | ko.Computed<string>;
                color: ko.Observable<string> | ko.Computed<string>;
                popoverVisible: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            enum ReportLayout {
                stepped = 0,
                block = 1,
                outline1 = 2,
                outline2 = 3,
                alignLeft1 = 4,
                alignLeft2 = 5,
                columnar = 6,
                tabular = 7,
                justified = 8
            }
            class LayoutTypeItem {
                layoutType: ReportLayout;
                margin: number;
                constructor(textValue: string, textID: string, layoutType: ReportLayout, margin: number);
                text: string;
                readonly imageClassName: string;
            }
            enum PageOrientation {
                Portrait = 0,
                Landscape = 1
            }
            class PageOrientationItem {
                orientation: PageOrientation;
                constructor(textValue: string, textID: string, orientation: PageOrientation);
                text: string;
            }
        }
        module Internal {
            interface IMasterDetailInfoBase {
                name: string;
                displayName: string;
                specifics: string;
            }
            interface IMasterDetailFieldInfo extends IMasterDetailInfoBase {
                checked: boolean;
            }
            interface IMasterDetailQueryInfo extends IMasterDetailInfoBase {
                path: string;
                checked: boolean;
                fields: IMasterDetailFieldInfo[];
                relations: IMasterDetailQueryInfo[];
            }
            class MasterDetailInfoBase implements IMasterDetailInfoBase {
                name: string;
                specifics: string;
                constructor(name: string, specifics: string, displayName?: string);
                displayName: string;
            }
            interface IMasterDetailReportTree {
                name: string;
                displayName?: string;
                path: string;
                fields: DevExpress.Analytics.Utils.IDataMemberInfo[];
                level: number;
            }
            class MasterDetailFieldInfo extends MasterDetailInfoBase implements IMasterDetailFieldInfo {
                constructor(field: Analytics.Wizard.Internal.FieldTreeNode);
                checked: boolean;
            }
            class MasterDetailQueryInfo extends MasterDetailInfoBase implements IMasterDetailQueryInfo {
                private _complexFields;
                private _complexRelations;
                private _expandComplexFieds;
                constructor(dataMember: Analytics.Wizard.Internal.DataMemberTreeNode);
                path: string;
                checked: boolean;
                fields: IMasterDetailFieldInfo[];
                relations: IMasterDetailQueryInfo[];
            }
            class DataMemberCustomCheckedTreeNode extends Analytics.Wizard.Internal.DataMemberTreeNode {
                constructor(name: string, displayName: string, specifics: string, isChecked: boolean, path: string, afterCheckToggled?: (node: Analytics.Wizard.Internal.DataMemberTreeNode) => void);
                setChecked(value: boolean): void;
            }
            class MasterDetailTreeListController extends Analytics.Widgets.Internal.DataMemberTreeListController {
                constructor(hideDataMemberSubItems: any);
                canSelect(value: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): boolean;
                hasItems(item: Analytics.Utils.IDataMemberInfo): boolean;
                hideDataMemberSubItems: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            class AvailableFieldsTreeListController extends Designer.Internal.FieldListController {
                constructor(rootItems: any);
                itemsFilter(item: Analytics.Wizard.Internal.DataMemberTreeNode): boolean;
                isDraggable(item: DevExpress.Analytics.Widgets.Internal.TreeListItemViewModel): boolean;
                rootItems: any;
            }
            interface ISummaryDataMemberInfo extends Analytics.Utils.IDataMemberInfo {
                path?: string;
                fields?: ISummaryDataMemberInfo[];
                parent?: {
                    path?: string;
                    displayName?: string;
                };
            }
            class SummaryInfo extends Analytics.Utils.Disposable {
                constructor();
                getOptions(options: any): any;
                field: ko.Observable<DevExpress.Analytics.Utils.IDataMemberInfo>;
                selectedItems: ko.ObservableArray<any>;
                functionNames: ko.ObservableArray<string>;
                visible: ko.Observable<boolean>;
                value: any;
            }
            class SummaryInfoFieldlist extends SummaryInfo {
                constructor();
                field: ko.Observable<ISummaryDataMemberInfo>;
                selectedPath: ko.Observable<string>;
                displayName: ko.Computed<string>;
            }
            class PageSetupHelper {
                static mm2px(val: number): number;
                static in2px(val: number): number;
                static px2mm(val: number): number;
                static px2in(val: number): number;
                static mm2in(val: number): number;
                static in2mm(val: number): number;
                static getConverter(from: Wizard.GraphicsUnit, to: Wizard.GraphicsUnit): (val: number) => number;
            }
        }
        module Wizard {
            enum ReportStyle {
                Bold = 0,
                Casual = 1,
                Compact = 2,
                Corporate = 3,
                Formal = 4
            }
            class ReportStyleItem {
                reportStyle: ReportStyle;
                constructor(textDefault: string, textID: string, reportStyle: ReportStyle);
                text: string;
                readonly className: string;
            }
        }
        module Internal {
            interface ISummaryOptions {
                columnName: string;
                flags: number;
            }
            class SummaryOptionsWrapper {
                private _name;
                private static _getNumber;
                constructor(name: string, displayName: string);
                columnName: string;
                avg: ko.Observable<boolean>;
                count: ko.Observable<boolean>;
                max: ko.Observable<boolean>;
                min: ko.Observable<boolean>;
                sum: ko.Observable<boolean>;
                getOptions(): ISummaryOptions;
            }
            function getFormattedValueInUnits(value: number, unit: Wizard.GraphicsUnit): string;
            interface IReportWizardData {
                report: ko.Observable | ko.Computed;
                availableDataSources: Analytics.Internal.IDataSourceInfo[];
                dataSourceRefs: Utils.IDataSourceRefInfo[];
                isReportServer?: boolean;
                disableCustomSql?: boolean;
                wizardSettings?: Utils.IReportWizardSettings;
            }
            function _createReportWizard(reportWizardOptions: Wizard._ReportWizardOptions): Wizard.FullscreenReportWizard | Wizard.ReportWizard | Wizard.LegacyReportWizard;
            class ListViewModel<T> {
                caption?: string;
                private _items;
                private _refreshActiveItem;
                activeItemArray: ko.ObservableArray<T>;
                constructor(caption?: string);
                readonly items: T[];
                activeItem: T;
                add(item: T): void;
                addRange(items: T[]): void;
                removeActiveItem(): void;
                removeAll(): void;
                setItems(items: T[]): void;
                moveUp(): void;
                moveDown(): void;
                readonly isEmpty: boolean;
                isMoveUpEnabled(): boolean;
                isMoveDownEnabled(): boolean;
            }
        }
        module Wizard {
            module Legacy {
                class ReportWizardModel extends Analytics.Wizard.Legacy.MultiQueryDataSourceWizardModel {
                    useMasterDetailBuilder: boolean;
                    private _dataSource;
                    private _groups;
                    private _reportTree;
                    private _fillTreeQueries;
                    constructor(useMasterDetailBuilder?: boolean, requestWrapper?: DevExpress.QueryBuilder.Utils.RequestWrapper);
                    applyReportViewModel(reportModel: Controls.ReportViewModel): void;
                    initDataBindSettings(model: Controls.ReportViewModel): void;
                    reportType: ReportType;
                    dataSource: Analytics.Internal.IDataSourceInfo;
                    dataMemberPath: ko.Observable<string>;
                    dataMember: ko.Observable<Analytics.Utils.IDataMemberInfo>;
                    fields: ko.ObservableArray<Analytics.Utils.IDataMemberInfo>;
                    masterDetailInfoCollection: ko.ObservableArray<Internal.IMasterDetailQueryInfo>;
                    getOnlyCheckedQueries: (queries?: Internal.IMasterDetailQueryInfo[]) => Internal.IMasterDetailQueryInfo[];
                    initPageSetup: (model: Controls.ReportViewModel) => void;
                    reportTree: () => Internal.IMasterDetailReportTree[];
                    groups: string[][];
                    masterDetailGroups: {
                        [key: string]: any;
                    };
                    summaryOptionsColumns: ko.ObservableArray<Analytics.Utils.IDataMemberInfo>;
                    summaryOptions: Internal.ISummaryOptions[];
                    masterDetailSummaryOptionsColumns: {
                        [key: string]: ko.ObservableArray<DevExpress.Analytics.Utils.IDataMemberInfo>;
                    };
                    masterDetailSummariesInfo: {
                        [key: string]: {
                            column: DevExpress.Analytics.Utils.IDataMemberInfo;
                            summaryFunctions: number[];
                        }[];
                    };
                    ignoreNullValuesForSummary: boolean;
                    fitFieldsToPage: boolean;
                    layout: ReportLayout;
                    portrait: boolean;
                    style: ReportStyle;
                    reportTitle: string;
                    labelDetails: ILabelDetails;
                    sqlDataSourceWizardModel: Analytics.Wizard.Legacy.MultiQueryDataSourceWizardModel;
                    pageSetup: IPageSetup;
                    reportModel: Controls.ReportViewModel;
                    colorScheme: ColorScheme;
                    initialDataSourceInfo: Analytics.Internal.IDataSourceInfo;
                    initialDataMember: string;
                    dataSourceType: Analytics.Wizard.DataSourceType;
                }
                class DataSourceWizard extends Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel> {
                    constructor(dataSources: ko.Observable<Analytics.Internal.IDataSourceInfo[]>, fieldListsCallback: Internal.IReportWizardFieldsCallback);
                    start(): void;
                    title: any;
                }
                class ReportWizard extends Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel> {
                    private _dataSources;
                    private _reportModel?;
                    static _masterDetailWizardHeight: string;
                    static _masterDetailWizardWidth: string;
                    static _masterDetailScrollViewHeight: string;
                    private _labelWizardData;
                    private _selectDataSourcePage;
                    private _cloneDataSources;
                    private static _cloneDataSources;
                    constructor(_dataSources: ko.Observable<Analytics.Internal.IDataSourceInfo[]> | ko.Computed<Analytics.Internal.IDataSourceInfo[]>, callbacks: Internal.IReportWizardCallbacks, connectionStrings: Analytics.Wizard.IDataSourceWizardConnectionStrings, hideDataMemberSubItems?: boolean, disableCustomSql?: boolean, useMasterDetailWizard?: boolean, _reportModel?: ko.Observable<Controls.ReportViewModel> | ko.Computed<Controls.ReportViewModel>);
                    start(data?: ReportWizardModel, finishCallback?: (model: ReportWizardModel) => JQueryPromise<boolean>, reportModel?: any): void;
                    startWithCustomDataSources(dataSources: Analytics.Internal.IDataSourceInfo[], data?: ReportWizardModel, finishCallback?: (model: ReportWizardModel) => JQueryPromise<boolean>, reportModel?: any): void;
                    title: any;
                    labelWizardData: () => JQueryPromise<{
                        labelProducts: ILabelProduct[];
                        paperKinds: IPaperKind[];
                        labelDetails: ILabelDetails[];
                    }>;
                    dataSources: ko.Observable<Analytics.Internal.IDataSourceInfo[]>;
                    container: typeof Analytics.Internal.getParentContainer;
                    finishCallback: (wizardModel: Analytics.Wizard.Legacy.IDataSourceWizardModel) => JQueryPromise<any>;
                    isMasterDetailWizard: boolean;
                }
                class AddGroupingLevelPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    template: string;
                    description: any;
                    fields: Internal.ListViewModel<string>;
                    groups: Internal.ListViewModel<{
                        fields: ko.ObservableArray<string>;
                    }>;
                    addNewGroup: () => void;
                    appendFieldsToGroup: () => void;
                    removeGroup: () => void;
                    isCreateGroupEnabled: ko.PureComputed<boolean>;
                    isAppendToGroupEnabled: ko.PureComputed<boolean>;
                    isRemoveGroupEnabled: ko.PureComputed<boolean>;
                    moveUp: () => void;
                    moveDown: () => void;
                    isMoveUpEnabled: ko.PureComputed<boolean>;
                    isMoveDownEnabled: ko.PureComputed<boolean>;
                    fieldDblClick: (field: any) => void;
                    fieldClick: (e: {
                        itemData: any;
                    }) => void;
                    groupDblClick: (group: any) => void;
                    groupClick: (e: {
                        itemData: any;
                    }) => void;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class ChooseReportLayoutPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    private _isGroupedReport;
                    private _reportLayoutTypes;
                    private _groupedRreportLayoutsTypes;
                    private _pageOriantation;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    template: string;
                    description: any;
                    toggleFitFieldsToPage: () => void;
                    selectedLayoutType: ko.Observable<LayoutTypeItem>;
                    fitFieldsToPage: ko.Observable<boolean>;
                    pageOrientationItems: PageOrientationItem[];
                    selectedPageOrientation: ko.Observable<PageOrientationItem>;
                    layoutTypeItems: ko.PureComputed<LayoutTypeItem[]>;
                    layoutTypeItemClick: (item: LayoutTypeItem) => void;
                    isSelected: (item: LayoutTypeItem) => boolean;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class ChooseReportStylePage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    template: string;
                    description: any;
                    reportStyleItems: ReportStyleItem[];
                    selectedReportStyle: ko.Observable<ReportStyleItem>;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class ChooseReportTypePage extends Analytics.Wizard.Legacy.ChooseDataSourceTypePage<ReportWizardModel> {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>, wizardSettings?: Utils.IReportWizardSettings);
                    description: any;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class ExtendChooseReportTypePage extends ChooseReportTypePage {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>, wizardSettings?: Utils.IReportWizardSettings);
                }
                class ChooseDataSourceTypePage extends Analytics.Wizard.Legacy.ChooseDataSourceTypePage<ReportWizardModel> {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>, wizardSettings?: Utils.IReportWizardSettings);
                }
                class ChooseSummaryOptionsPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    private _columns;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    template: string;
                    description: any;
                    summaryOptions: ko.ObservableArray<Internal.SummaryOptionsWrapper>;
                    ignoreNullValues: ko.Observable<boolean>;
                    toggleIgnoreNullValues: () => void;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class ColorSchemePage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    applyScheme(data: ColorScheme): void;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    reset(): void;
                    template: string;
                    description: any;
                    scheme: ko.Observable<ColorScheme> | ko.Computed<ColorScheme>;
                    customColorScheme: CustomColorScheme;
                    lookupData: {
                        scheme: ColorScheme[];
                    };
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class CustomizeLabelPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    wizard: any;
                    static CONVERSION_COEEFICIENT: number;
                    private _id;
                    private _labelWidth;
                    private _labelHeight;
                    private _horizontalPitch;
                    private _verticalPitch;
                    private _topMargin;
                    private _leftMargin;
                    private _rightMargin;
                    private _bottomMargin;
                    private _getFormattedValueInUnits;
                    private _getOtherMarginValue;
                    private _getLabelsCount;
                    private _rowsCount;
                    private _columnsCount;
                    private _pageHeight;
                    private _pageWidth;
                    constructor(wizard: any);
                    template: string;
                    description: any;
                    labelData: {
                        labelProducts: ILabelProduct[];
                        paperKinds: IPaperKind[];
                        labelDetails: ILabelDetails[];
                    };
                    paperKinds: () => IPaperKind[];
                    selectedPaperSize: ko.Observable<IPaperKind>;
                    unit: ko.Observable<GraphicsUnit>;
                    stepUnit: ko.PureComputed<0.1 | 0.01>;
                    labelWidth: ko.Computed<number>;
                    labelHeight: ko.Computed<number>;
                    horizontalPitch: ko.Computed<number>;
                    verticalPitch: ko.Computed<number>;
                    topMargin: ko.Computed<number>;
                    leftMargin: ko.Computed<number>;
                    rightMargin: ko.Computed<number>;
                    bottomMargin: ko.Computed<number>;
                    labelsCountText: ko.PureComputed<string>;
                    pageSizeText: ko.PureComputed<string>;
                    static getPageSizeText(width: number, height: number, unit: Designer.Wizard.GraphicsUnit): string;
                    units: {
                        text: any;
                        value: GraphicsUnit;
                    }[];
                    beginAsync(data: ReportWizardModel): any;
                    commit(data: ReportWizardModel): void;
                }
                class MasterDetailAddGroupingLevel extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    private _availableColumns;
                    private _groupingLevels;
                    private _isModelChanged;
                    private _modelChanged;
                    private _setData;
                    private _masterDetailGroups;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    template: string;
                    description: any;
                    addNewGroup: () => void;
                    appendFieldsToGroup: () => void;
                    removeGroup: () => void;
                    isCreateGroupEnabled: ko.PureComputed<boolean>;
                    isAppendToGroupEnabled: ko.PureComputed<boolean>;
                    isRemoveGroupEnabled: ko.PureComputed<boolean>;
                    moveUp: () => void;
                    moveDown: () => void;
                    isMoveUpEnabled: ko.PureComputed<boolean>;
                    isMoveDownEnabled: ko.PureComputed<boolean>;
                    fieldDblClick: (field: any) => void;
                    fieldClick: (e: {
                        itemData: any;
                    }) => void;
                    groupDblClick: (group: any) => void;
                    groupClick: (e: {
                        itemData: any;
                    }) => void;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                    reset(): void;
                    currentPath: ko.Observable<string>;
                    currentFields: ko.Observable<Designer.Internal.ListViewModel<string>>;
                    currentGroups: ko.Observable<Designer.Internal.ListViewModel<{
                        fields: ko.ObservableArray<string>;
                    }>>;
                    fieldCaption: any;
                    groupCaption: any;
                    reportTree: ko.ObservableArray<Internal.IMasterDetailReportTree>;
                }
                class MasterDetailChooseSummaryOptions extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    private _allColumns;
                    private _masterDetailColumns;
                    private _currentDataMember;
                    private _createSummaryInfo;
                    private _createNewItemIfNeed;
                    private _changeQuery;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    removeSummaryInfo(info: Internal.SummaryInfo): void;
                    toggleIgnoreNullValues: () => void;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                    reset(): void;
                    summaryOptions: ko.ObservableArray<Internal.SummaryOptionsWrapper>;
                    ignoreNullValues: ko.Observable<boolean>;
                    template: string;
                    description: any;
                    reportTree: ko.ObservableArray<Internal.IMasterDetailReportTree>;
                    currentPath: ko.Observable<string>;
                    availableFields: ko.ObservableArray<any>;
                    displayedFields: {
                        [key: string]: ko.ObservableArray<any>;
                    };
                    summaryInfos: ko.ObservableArray<Internal.SummaryInfo>;
                    summaryInfoMapByDataMember: {
                        [key: string]: Internal.SummaryInfo[];
                    };
                    selectFieldToSummaryCaption: any;
                    fieldsCaption: any;
                    summaryFunctionCaption: any;
                    ignoreNullValuesCaption: any;
                }
                class MasterDetailSelectReportDataPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    private _rootItems;
                    private _dataMemberSelectedPath;
                    private _fieldSelectedPath;
                    private _fieldListCallBack;
                    private _createSqlDataSourceInfo;
                    private _createJsonDataSourceInfo;
                    private _dataMemberItemsProvider;
                    private _fieldMemberItemsProvider;
                    private _availableFieldsController;
                    private _dataSource;
                    private _hideDataMemberSubItems;
                    private _checkedDataMembers;
                    private _checkedFields;
                    private _data;
                    private initialFullDataMember;
                    private _wrapFieldListCallback;
                    private readonly dataSourcePath;
                    private _showDataSource;
                    private getDataMemberSelectedPath;
                    private _beginInternal;
                    private _resetDataAfterCheck;
                    private _afterCheckToggled;
                    private _processFields;
                    private _processNode;
                    private _afterCheckToggledFields;
                    private _createMasterDetailTreeNode;
                    private _createMasterDetailFirstTabTreeNode;
                    private _createMasterDetailLeafTreeNode;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>, fieldListItemsCallback: Internal.IReportWizardFieldsCallback, createSqlDataSourceInfo: (dataSource: Analytics.Data.SqlDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>, createJsonDataSourceInfo: (dataSource: Analytics.Data.JsonDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>, hideDataMemberSubItems?: boolean);
                    template: string;
                    description: any;
                    dataMemberFieldListModel: Analytics.Widgets.Internal.ITreeListOptions;
                    fieldMemberFieldListModel: Analytics.Widgets.Internal.ITreeListOptions;
                    beginAsync(data: ReportWizardModel): JQueryPromise<any>;
                    commit(data: ReportWizardModel): void;
                    reset(): void;
                    showFirstLevelDataMembers: ko.Observable<boolean>;
                    selectDataMembersCaption: any;
                    selectDataFieldsCaption: any;
                }
                class PageSetupPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    reset(): void;
                    template: string;
                    description: any;
                    paperKind: ko.Observable<string> | ko.Computed<string>;
                    landscape: ko.Observable<boolean> | ko.Computed<boolean>;
                    unit: ko.Observable<GraphicsUnit> | ko.Computed<GraphicsUnit>;
                    width: ko.Observable<number> | ko.Computed<number>;
                    height: ko.Observable<number> | ko.Computed<number>;
                    fixedSize: ko.Observable<boolean> | ko.Computed<boolean>;
                    marginLeft: ko.Observable<number> | ko.Computed<number>;
                    marginRight: ko.Observable<number> | ko.Computed<number>;
                    marginTop: ko.Observable<number> | ko.Computed<number>;
                    marginBottom: ko.Observable<number> | ko.Computed<number>;
                    previewAreaWidth: ko.Observable<number> | ko.Computed<number>;
                    previewAreaHeight: ko.Observable<number> | ko.Computed<number>;
                    previewPageWidth: ko.Observable<number> | ko.Computed<number>;
                    previewPageHeight: ko.Observable<number> | ko.Computed<number>;
                    previewPageTop: ko.Observable<number> | ko.Computed<number>;
                    previewPageLeft: ko.Observable<number> | ko.Computed<number>;
                    previewTopMargin: ko.Observable<number> | ko.Computed<number>;
                    previewRightMargin: ko.Observable<number> | ko.Computed<number>;
                    previewBottomMargin: ko.Observable<number> | ko.Computed<number>;
                    previewLeftMargin: ko.Observable<number> | ko.Computed<number>;
                    valueFormat: ko.Observable<string> | ko.Computed<string>;
                    lookupData: {
                        paperKind: {
                            value: string;
                            displayName: string;
                        }[];
                        unit: {
                            value: GraphicsUnit;
                            displayName: string;
                        }[];
                    };
                    private _unit;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class DataSourceParameterWrapper {
                    private static _itemsProvider;
                    constructor(parameter: Analytics.Data.DataSourceParameter);
                    isValid: ko.Observable<boolean> | ko.Computed<boolean>;
                    parameter: Analytics.Data.DataSourceParameter;
                    name: ko.Computed<string>;
                    value: ko.Observable | ko.Computed;
                    type: ko.Observable<string> | ko.Computed<string>;
                    readonly specifics: any;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                }
                class ReportWizardConfigureParametersPage extends Analytics.Wizard.Legacy.MultiQueryConfigureParametersPage {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<Analytics.Wizard.Legacy.MultiQueryDataSourceWizardModel>);
                    beginAsync(data: ReportWizardModel): JQueryPromise<any>;
                    commit(data: ReportWizardModel): void;
                }
                class SelectColumnsPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    private _fieldListsCallback;
                    private _selectedPath;
                    private _fields;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>, getFieldListItems: Internal.IReportWizardFieldsCallback);
                    template: string;
                    description: any;
                    availableFields: Internal.ListViewModel<Analytics.Utils.IDataMemberInfo>;
                    selectedFields: Internal.ListViewModel<Analytics.Utils.IDataMemberInfo>;
                    isSelectEnable: ko.PureComputed<boolean>;
                    isUnselectEnable: ko.PureComputed<boolean>;
                    select: () => void;
                    selectAll: () => void;
                    unselect: () => void;
                    unselectAll: () => void;
                    availableFieldDblClick: (field: any) => void;
                    availableFieldClick: (e: {
                        itemData: any;
                    }) => void;
                    selectedFieldDblClick: (field: any) => void;
                    selectedFieldClick: (e: {
                        itemData: any;
                    }) => void;
                    beginAsync(data: ReportWizardModel): JQueryPromise<any>;
                    commit(data: ReportWizardModel): void;
                    selectedPath(): any;
                    reset(): void;
                }
                class SelectDataMemberPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    private _rootItems;
                    private _selectedPath;
                    private _fieldListCallBack;
                    private _createSqlDataSourceInfo;
                    private _dataSource;
                    private _hideDataMemberSubItems;
                    private _getSelectedDataMember;
                    private _wrapFieldListCallback;
                    private readonly dataSourcePath;
                    private _beginInternal;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>, fieldListItemsCallback: Internal.IReportWizardFieldsCallback, createSqlDataSourceInfo: (dataSource: Analytics.Data.SqlDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>, hideDataMemberSubItems?: boolean);
                    template: string;
                    description: any;
                    scrollViewHeight: string;
                    fieldListModel: {
                        itemsProvider: Analytics.Internal.FieldListProvider;
                        selectedPath: any;
                        treeListController: Analytics.Widgets.Internal.DataMemberTreeListController;
                    };
                    beginAsync(data: ReportWizardModel): JQueryPromise<any>;
                    commit(data: ReportWizardModel): void;
                }
                class SelectDataSourcePage extends Analytics.Wizard.Legacy.SelectOptionalConnectionString<ReportWizardModel> {
                    private _getSelectedDataSource;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>, availableDataSources: ko.Observable<Analytics.Internal.IDataSourceInfo[]> | ko.Computed<Analytics.Internal.IDataSourceInfo[]>, isDataSourceCreationAvailable: ko.Observable<boolean> | ko.Computed<boolean>);
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                    getSelectedDataSource(data: ReportWizardModel): any[];
                    readonly createNewDataSourceOperationText: any;
                }
                class SelectPredefinedLabelsPage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    wizard: any;
                    constructor(wizard: any);
                    template: string;
                    description: any;
                    labelData: {
                        labelProducts: ILabelProduct[];
                        paperKinds: IPaperKind[];
                        labelDetails: ILabelDetails[];
                    };
                    selectedLabelProduct: ko.Observable<ILabelProduct>;
                    selectedLabelDetails: ko.Observable<ILabelDetails>;
                    labelDetails: ko.Observable<any>;
                    selectedPaperSize: ko.Computed<IPaperKind>;
                    width: ko.PureComputed<string>;
                    height: ko.PureComputed<string>;
                    paperType: ko.PureComputed<string>;
                    pageSizeText: ko.PureComputed<string>;
                    beginAsync(data: ReportWizardModel): any;
                    commit(data: ReportWizardModel): void;
                }
                class SetReportTitlePage extends Analytics.Wizard.Legacy.WizardPage<ReportWizardModel> {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<ReportWizardModel>);
                    template: string;
                    description: any;
                    reportTitle: ko.Observable<string>;
                    _begin(data: ReportWizardModel): void;
                    commit(data: ReportWizardModel): void;
                }
                class ReportWizardSelectConnectionString extends Analytics.Wizard.Legacy.SelectConnectionString<Analytics.Wizard.Legacy.MultiQueryDataSourceWizardModel> {
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<Analytics.Wizard.Legacy.MultiQueryDataSourceWizardModel>, connectionStrings: ko.ObservableArray<Analytics.Wizard.IConnectionStringDefinition>);
                    beginAsync(data: ReportWizardModel): JQueryPromise<any>;
                    commit(data: ReportWizardModel): void;
                }
                class ReportWizardAddQueriesPage extends Analytics.Wizard.Legacy.MultiQueryConfigurePage {
                    private _reportData;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<Analytics.Wizard.Legacy.MultiQueryDataSourceWizardModel>, callbacks: Analytics.Wizard.Internal.IDataSourceWizardCallbacks, disableCustomSql: boolean, rtl: boolean);
                    beginAsync(data: ReportWizardModel): JQueryPromise<any>;
                    commit(data: ReportWizardModel): void;
                    subscribeCheckedItems: ko.Observable<any>;
                }
                class ReportWizardMasterDetailRelationsPage extends Analytics.Wizard.Legacy.MasterDetailRelationsPage {
                    private _reportData;
                    constructor(wizard: Analytics.Wizard.Legacy.WizardViewModel<Analytics.Wizard.Legacy.MultiQueryDataSourceWizardModel>, sqlDataSourceResultSchema: (dataSource: Analytics.Data.SqlDataSource) => JQueryPromise<{
                        resultSchemaJSON: string;
                        connectionParameters?: string;
                    }>);
                    beginAsync(data: ReportWizardModel): JQueryPromise<any>;
                    commit(data: ReportWizardModel): void;
                    subscribeVisible: ko.Observable<boolean>;
                }
            }
        }
        module Tools {
            interface IDialogModel {
                getUrl: () => string;
                setUrl: (url: string) => void;
                onShow: (tab: INavigateTab) => void;
                popupButtons: any[];
            }
            class ReportDialogBase extends Analytics.Utils.Disposable {
                dispose(): void;
                show(tab: INavigateTab): void;
                customize(template: string, model: IDialogModel): void;
                constructor();
                width: ko.Observable<any>;
                height: ko.Observable<any>;
                template: ko.Observable<string>;
                buttons: any[];
                model: ko.Observable<IDialogModel>;
                tab: ko.Observable<INavigateTab>;
                visible: ko.Observable<boolean>;
                cancel(): void;
                container: (element: HTMLElement) => JQuery;
            }
            class OpenReportDialogModelBase implements IDialogModel {
                urls: any;
                constructor(popup: OpenReportDialog, urls: any);
                onShow(tab: INavigateTab): void;
                getUrl(): string;
                setUrl(url: any): void;
                searchValue: ko.Observable<string>;
                searchPlaceholder: () => any;
                popupButtons: any[];
                reportUrl: ko.Observable<string>;
                noDataText: any;
            }
            class OpenReportDialog extends ReportDialogBase {
                title: string;
                open(url: string): void;
                constructor(subreports: any, navigateByReports: NavigateByReports, callbacks: Utils.IReportDesignerCustomizationHandler);
                navigateByReports: NavigateByReports;
                onOpening: (e: any) => void;
                onOpened: (e: any) => void;
            }
            class SaveAsReportDialogModelBase implements IDialogModel {
                onShow(tab: INavigateTab): void;
                constructor(popup: SaveAsReportDialog, urls: ko.ObservableArray<IKeyValuePair<string>>);
                getUrl(): string;
                setUrl(url: any): void;
                popupButtons: any[];
                reportUrl: ko.Observable<string> | ko.Computed<string>;
                noDataText: any;
                reportNamePlaceholder: () => any;
                urls: ko.ObservableArray<IKeyValuePair<string>>;
                reportName: ko.Observable<string> | ko.Computed<string>;
            }
            class SaveAsReportDialog extends ReportDialogBase {
                show(tab: INavigateTab): void;
                constructor(subreports: ko.ObservableArray<IKeyValuePair<string>>, callbacks: Utils.IReportDesignerCustomizationHandler);
                save(url: any): void;
                onSaving: (e: any) => void;
                onSaved: (e: any) => void;
                closeAfterSave: ko.Observable<boolean>;
                title: string;
            }
            class SaveReportDialogModelBase implements IDialogModel {
                onShow(tab: INavigateTab): void;
                getUrl(): string;
                setUrl(url: any): void;
                constructor(popup: SaveReportDialog);
                popupButtons: any[];
                reportUrl: ko.Observable<string>;
                saveText: ko.Observable<string>;
            }
            class SaveReportDialog extends ReportDialogBase {
                constructor(saveReportDialog: SaveAsReportDialog, callbacks: Utils.IReportDesignerCustomizationHandler);
                save(url: any): void;
                notSave(): void;
                cancel(): void;
                saveReportDialog: SaveAsReportDialog;
                onSaving: (e: any) => void;
                onSaved: (e: any) => void;
                title: string;
            }
        }
        module Internal {
            function isHeaderOrFooterBandType(band: any): boolean;
            function bandContainsToc(reportModel: Controls.ReportViewModel, bandType: string): any;
            function bandControlsSomeXRTableOfContents(band: any): any;
            class ChartFieldListExtender implements DevExpress.Analytics.Internal.IItemsExtender {
                beforeItemsFilled(request: DevExpress.Analytics.Utils.IPathRequest, items: DevExpress.Analytics.Utils.IDataMemberInfo[]): boolean;
            }
            var createChartDesignerOptions: (designerModel: any, dataSourceHelper: any, model: any, parameters: any, chartValueBindingProvider: any) => {
                dispose: () => void;
                options: Chart.Internal.IChartDesignerOptions;
                visible: ko.Observable<boolean>;
                buttons: {
                    toolbar: string;
                    location: string;
                    widget: string;
                    options: {
                        text: any;
                        onClick: () => void;
                    };
                }[];
                run: (chartSurface: Controls.XRChartSurface) => void;
                container: (element: HTMLElement) => JQuery;
            };
            class DataFilterModelReport extends Chart.Internal.Models.DataFilterModel {
                getInfo(): any[];
                private _createReportDataProperty;
                private _mapObject;
                constructor(model: any, serializer?: any);
                dataMember: ko.Observable<string> | ko.Computed<string>;
                dataSource: ko.Observable<any>;
                misc: any;
                report: any;
            }
            function subreportControlCollector(target: any, subreportControls?: any[]): any[];
            interface ITableCalculationNode {
                column: number;
                row: number;
                calc: () => void;
            }
            class TableCalculationProvider {
                private _table;
                private _tableOffset;
                private _calculationStarted;
                private _calculationTimeout;
                private _calculationNodes;
                private _resetState;
                private _startCalculation;
                constructor(_table: Controls.XRTableControlViewModel);
                addTableOffset(width: any, left: any): void;
                addCalculationNode(node: ITableCalculationNode): void;
                hasCalculationNode(rowIndex: number, cellIndex: number): boolean;
            }
        }
        module Controls {
            module PivotGrid {
                class SortBySummaryInfoCondition implements Analytics.Utils.ISerializableModel {
                    private _fieldsProvider;
                    constructor(model: any, fieldsProvider: {
                        fieldsAvailableForCondition: () => string[];
                    }, serializer?: Analytics.Utils.IModelSerializer);
                    fieldComponentName: ko.Observable<string> | ko.Computed<string>;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    static createNew(parent: SortBySummaryInfo, serializer?: Analytics.Utils.IModelSerializer): SortBySummaryInfoCondition;
                }
                class SortBySummaryInfo {
                    private _field;
                    private _pivotGridFields;
                    constructor(model: any, field: PivotGridFieldViewModel, serializer?: Analytics.Utils.IModelSerializer);
                    conditions: ko.ObservableArray<SortBySummaryInfoCondition>;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    fieldsAvailableForCondition(): string[];
                    static from(model: any, serializer?: Analytics.Utils.IModelSerializer): any;
                    static toJSON(viewModel: SortBySummaryInfo, serializer?: Analytics.Utils.IModelSerializer, refs?: any): any;
                }
                module Metadata {
                    var summaryTypeValues: Array<Analytics.Utils.IDisplayedValue>;
                    var summaryType: Analytics.Utils.ISerializationInfo;
                    var fieldComponentName: Analytics.Utils.ISerializationInfo;
                    var conditions: Analytics.Utils.ISerializationInfo;
                    var field: Analytics.Utils.ISerializationInfo;
                    var customTotalSummaryType: Analytics.Utils.ISerializationInfo;
                    var sortBySummaryInfo: Analytics.Utils.ISerializationInfoArray;
                    var sortBySummaryConditionInfo: Analytics.Utils.ISerializationInfoArray;
                }
            }
            class XRReportElementViewModel extends Analytics.Elements.ElementViewModel {
                dispose(): void;
                private _getStylePriorityPropertyName;
                private _getStyle;
                private _checkModify;
                private _getStyleProperty;
                private _zOrderChange;
                private _createPaddingDependencies;
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.IModelSerializer);
                getControlFactory(): ControlsFactory;
                addChild(control: Analytics.Elements.IElementViewModel): void;
                initDataBindingProperties(): void;
                initExpressionProperties(): void;
                initBindings(): void;
                dsHelperProvider: () => Internal.DataSourceHelper;
                isStyleProperty(propertyName: string): boolean;
                isResettableProperty(propertyName: string): boolean;
                getActionClassName(propertyName: string): string;
                className(): string;
                initialize(): void;
                getPath(propertyName: any): string;
                isPropertyDisabled(name: string): boolean;
                isPropertyVisible(name: string): boolean;
                sendToBack(): void;
                bringToFront(): void;
                getControlContainerName(): string;
                readonly dataBindingMode: any;
                dpi: ko.Observable<number> | ko.Computed<number>;
                _innerDpi: ko.Observable<number> | ko.Computed<number>;
                styleName: ko.Observable<string> | ko.Computed<string>;
                stylePriority: {
                    [key: string]: ko.Observable<boolean> | ko.Computed<boolean>;
                };
                formattingRuleLinks: ko.ObservableArray<FormattingRuleLink>;
                dataBindings: ko.ObservableArray<Data.DataBinding>;
                size: Analytics.Elements.Size;
                location: Analytics.Elements.Point;
                scripts: any;
                paddingObj: Analytics.Elements.PaddingModel;
                expressionBindings: ko.ObservableArray<IExpressionBinding>;
                expressionObj: Internal.IExpressionObject;
                padding: ko.Observable<string> | ko.Computed<string>;
                root: XRReportElementViewModel;
                getStyleProperty: (propertyName: string, styleProperty: string) => any;
                toggleUseStyle: (propertyName: string) => void;
                _lockedInUserDesigner: ko.Observable<boolean> | ko.Computed<boolean>;
                lockedInUserDesigner: ko.Computed<boolean>;
                rtl(): boolean;
                parentModel: ko.Observable<Bands.BandViewModel | any>;
            }
            class XRControlViewModel extends XRReportElementViewModel {
                dispose(): void;
                static getNearestBand(target: Analytics.Elements.IElementViewModel): Bands.BandViewModel;
                anchoring(parent: IAnchoringProperties): void;
                constructor(control: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.IModelSerializer);
                getNearestParent(target: Analytics.Elements.IElementViewModel): any;
                isPropertyDisabled(name: any): any;
                isPropertyVisible(name: any): boolean;
                hasExpressionBindings(): boolean;
                hasDataBindingByName(property?: string): boolean;
                readonly hasDefaultBindingProperty: boolean;
                getExpressionBinding(property?: string, event?: string): string;
                setExpressionBinding(value: string, property?: string, event?: string): void;
                getControlInfo(): IReportControlMetadata;
                getDefaultBinding(): Analytics.Widgets.IExpressionOptions | Data.DataBinding;
                textArea: ko.Observable<string> | ko.Computed<string>;
                multiline: ko.Observable<boolean> | ko.Computed<boolean>;
                name: ko.Observable<string> | ko.Computed<string>;
                text: ko.Observable<string> | ko.Computed<string>;
                textFormatString: ko.Observable<string> | ko.Computed<string>;
                controls: ko.ObservableArray<XRControlViewModel>;
                popularDataBinding: any;
                anchorVertical: ko.Observable<string> | ko.Computed<string>;
                anchorHorizontal: ko.Observable<string> | ko.Computed<string>;
                vertAnchoring: VerticalAcnhoring;
                horAnchoring: HorizontalAnchoring;
                hasBindings: ko.Computed<boolean>;
                interactiveSorting: SortingOptions;
                expressionBindings: ko.ObservableArray<IExpressionBinding>;
                dataBindingsAreValid: ko.Observable<boolean> | ko.Computed<boolean>;
            }
            class XRControlSurfaceBase<M extends Analytics.Elements.ElementViewModel> extends Analytics.Elements.SurfaceElementBase<M> {
                private delta;
                private _isThereIntersectionWithUsefulArea;
                static _appendValue(accumulator: string, value: string, needToAppend?: boolean): string;
                constructor(control: M, context: Analytics.Elements.ISurfaceContext, unitProperties: Analytics.Internal.IUnitProperties<M>);
                checkParent(surfaceParent: Analytics.Internal.ISelectionTarget): boolean;
                isThereIntersection(rect1: Analytics.Elements.IArea, rect2: Analytics.Elements.IArea): boolean;
                isThereIntersectionWithParent(parentRect: Analytics.Elements.IArea, childRect: Analytics.Elements.IArea): boolean;
                isThereIntersectionWithUsefulArea(): boolean;
                isThereIntersectionWithCrossBandControls(currentRect?: Analytics.Elements.IArea): boolean;
                isThereIntersectionWithControls(): boolean;
                isThereIntersectionWithParentCollection(currentRect: any, controlRectProperty?: string): boolean;
                isThereIntersectionWithChildCollection(currentRect: any, controlRectProperty?: string): boolean;
                isThereIntersectionWithNeighborsCollection(currentRect: any, collectionControls: any, controlRectProperty?: string): boolean;
                isThereIntersectionWithChildControls(currentRect: any, collectionControls: any, controlRectProperty?: string): boolean;
                getAdornTemplate(): string;
                hasDataBindingByName(propertyName: string): boolean;
                readonly hasBindings: boolean;
                readonly bindingsIsValid: any;
                contentSizes: any;
                contentHeightWithoutZoom: any;
                contentWidthWithoutZoom: any;
                borderCss: any;
                template: string;
                selectiontemplate: string;
                contenttemplate: string;
                isIntersect: ko.Computed<boolean>;
                adorntemplate: ko.Computed<string>;
                displayNameParameters: ko.PureComputed<{
                    text: any;
                    isExpression: boolean;
                    dataSource: any;
                    dataMember: any;
                    dataMemberOffset: any;
                    allowMarkupText: boolean;
                    wordWrap: boolean;
                    fontSize: number;
                    fontUnit: any;
                }>;
                displayName: ko.PureComputed<any>;
                displayText(): any;
            }
            class XRControlSurface extends XRControlSurfaceBase<XRControlViewModel> {
                dispose(): void;
                static _unitProperties: Analytics.Internal.IUnitProperties<XRControlViewModel>;
                constructor(control: XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
                controls: ko.ObservableArray<XRControlSurface>;
            }
            module Metadata {
                var panelSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            module PivotGrid {
                interface IPivotGridField {
                    area: ko.Observable<string> | ko.Computed<string>;
                    areaIndex: ko.Observable<number> | ko.Computed<number>;
                }
                class PivotGridFieldViewModel extends Analytics.Elements.ElementViewModel implements IPivotGridField {
                    static fieldHeight: number;
                    static createNew(parent: any): () => PivotGridFieldViewModel;
                    getInfo(): Analytics.Utils.ISerializationInfoArray;
                    getControlFactory(): ControlsFactory;
                    constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                    getPath(propertyName: any): string;
                    getDisplayName(): any;
                    controlType: string;
                    area: ko.Observable<string> | ko.Computed<string>;
                    areaIndex: ko.Observable<number> | ko.Computed<number>;
                    areaIndexEditable: ko.Observable<number> | ko.Computed<number>;
                    index: ko.Observable<number> | ko.Computed<number>;
                    fieldName: ko.Observable<string> | ko.Computed<string>;
                    fieldNameEditable: any;
                    caption: ko.Observable<string> | ko.Computed<string>;
                    sortBySummaryInfo: SortBySummaryInfo;
                }
                class PivotGridFieldSurface extends Controls.XRControlSurfaceBase<PivotGridFieldViewModel> implements IPivotGridField {
                    constructor(control: PivotGridFieldViewModel, context: Analytics.Elements.ISurfaceContext);
                    minWidth: ko.Computed<number>;
                    area: ko.Observable<string> | ko.Computed<string>;
                    areaIndex: ko.Observable<number> | ko.Computed<number>;
                    positionWidthWithoutZoom: ko.Computed<number>;
                }
                module Metadata {
                    var caption: Analytics.Utils.ISerializationInfo;
                    var index: Analytics.Utils.ISerializationInfo;
                    var fieldName: Analytics.Utils.ISerializationInfo;
                    var minWidth: Analytics.Utils.ISerializationInfo;
                    var width: Analytics.Utils.ISerializationInfo;
                    var area: Analytics.Utils.ISerializationInfo;
                    var allowedAreas: Analytics.Utils.ISerializationInfo;
                    var areaIndex: Analytics.Utils.ISerializationInfo;
                    var areaIndexEditable: Analytics.Utils.ISerializationInfo;
                    var unboundType: Analytics.Utils.ISerializationInfo;
                    var unboundFieldName: Analytics.Utils.ISerializationInfo;
                    var unboundExpression: Analytics.Utils.ISerializationInfo;
                    var topValueType: Analytics.Utils.ISerializationInfo;
                    var topValueShowOthers: Analytics.Utils.ISerializationInfo;
                    var topValueCount: Analytics.Utils.ISerializationInfo;
                    var summaryDisplayType: Analytics.Utils.ISerializationInfo;
                    var sortOrder: Analytics.Utils.ISerializationInfo;
                    var sortMode: Analytics.Utils.ISerializationInfo;
                    var showNewValues: Analytics.Utils.ISerializationInfo;
                    var runningTotal: Analytics.Utils.ISerializationInfo;
                    var rowValueLineCount: Analytics.Utils.ISerializationInfo;
                    var groupIntervalNumericRange: Analytics.Utils.ISerializationInfo;
                    var groupInterval: Analytics.Utils.ISerializationInfo;
                    var grandTotalText: Analytics.Utils.ISerializationInfo;
                    var expandedInFieldsGroup: Analytics.Utils.ISerializationInfo;
                    var emptyValueText: Analytics.Utils.ISerializationInfo;
                    var emptyCellText: Analytics.Utils.ISerializationInfo;
                    var displayFolder: Analytics.Utils.ISerializationInfo;
                    var columnValueLineCount: Analytics.Utils.ISerializationInfo;
                    var totalsVisibility: Analytics.Utils.ISerializationInfo;
                    var useNativeFormat: Analytics.Utils.ISerializationInfo;
                    var KPIGraphic: Analytics.Utils.ISerializationInfo;
                    var cellFormat: Analytics.Utils.ISerializationInfo;
                    var totalCellFormat: Analytics.Utils.ISerializationInfo;
                    var grandTotalCellFormat: Analytics.Utils.ISerializationInfo;
                    var valueFormat: Analytics.Utils.ISerializationInfo;
                    var totalValueFormat: Analytics.Utils.ISerializationInfo;
                    var appearanceInfo: Analytics.Utils.ISerializationInfoArray;
                    var appearancesInfo: Analytics.Utils.ISerializationInfoArray;
                    var unboundExpressionMode: Analytics.Utils.ISerializationInfo;
                    var options: Analytics.Utils.ISerializationInfo;
                    var sortBySummary: Analytics.Utils.ISerializationInfo;
                    var pivotGridFieldSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                    var popularPropertiesPivotGridField: string[];
                    var pivotGridFieldsSerializable: Analytics.Utils.ISerializationInfo;
                }
            }
            class XRTextControlSurfaceBase<M extends Analytics.Elements.ElementViewModel> extends XRControlSurfaceBase<M> {
                private _$element;
                private _font;
                characterHeight: ko.Computed<number>;
                contenttemplate: string;
                getAlignments(): {
                    vertical: string;
                    horizontal: string;
                };
                getWordWrap(): any;
                getCssContent(content?: Object): Object;
                getContentSize(): any;
                getText(): string;
                getFontModel(): Analytics.Widgets.Internal.FontModel;
                setFontSize(size: any): void;
                cacheElementContent($element: JQuery): void;
                fitTextToBounds(): void;
                fitWidthToText(): void;
                fitHeightToText(): void;
                fitBoundsToText(): void;
                constructor(control: M, context: Analytics.Elements.ISurfaceContext, units?: Analytics.Internal.IUnitProperties<any>);
                fitTextToBoundsAction: Actions.FitTextToBoundsAction;
                fitBoundsToTextAction: Actions.FitBoundsToTextAction;
            }
            module Metadata {
                var labelSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesLabel: string[];
            }
        }
        module Internal {
            enum TableActionDirection {
                vertical = 0,
                horizontal = 1
            }
            class TableComponentSurface<T extends Analytics.Elements.ElementViewModel> extends Controls.XRTextControlSurfaceBase<T> {
                private _getNeededProperties;
                private _generateRect;
                beforeRectUpdated(rect: Analytics.Elements.IArea): Analytics.Elements.IArea;
                direction: TableActionDirection;
            }
            class TodoControlSurface extends Controls.XRControlSurface {
                constructor(control: Controls.XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
            }
        }
        module Controls {
            class XRBarCodeViewModel extends XRControlViewModel {
                static unitProperties: string[];
                static barCodesTypes: Array<DevExpress.Analytics.Utils.IDisplayedValue>;
                createBarcode(model: any, serializer?: any): {
                    "name": ko.Observable<any>;
                    "getInfo": () => Analytics.Utils.ISerializationInfoArray;
                };
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                symbology: any;
                barcodeFake: any;
            }
            class XRBarcodeSurface extends XRControlSurface {
                constructor(control: XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
            }
            module Metadata {
                var autoModule: Analytics.Utils.ISerializationInfo;
                var barCodeOrientation: Analytics.Utils.ISerializationInfo;
                var moduleInfo: Analytics.Utils.ISerializationInfo;
                var showText: Analytics.Utils.ISerializationInfo;
                var symbology: Analytics.Utils.ISerializationInfo;
                var barcodeFake: Analytics.Utils.ISerializationInfo;
                var alignment: Analytics.Utils.ISerializationInfo;
                var barCodesMap: {
                    [key: string]: Analytics.Utils.ISerializationInfoArray;
                };
                var barcodeSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesBarCode: string[];
            }
            class XRCharacterComb extends XRControlViewModel {
                static unitProperties: string[];
                isPropertyDisabled(name: string): any;
                private _createCellSideFromOriginalSide;
                constructor(control: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.IModelSerializer);
                roundSize(): void;
                font: ko.Observable<string> | ko.Computed<string>;
                cellWidth: ko.Computed<number>;
                cellHeight: ko.Computed<number>;
                autoCellSide: ko.Observable<number> | ko.Computed<number>;
                verticalSpacing: ko.Observable<number> | ko.Computed<number>;
                horizontalSpacing: ko.Observable<number> | ko.Computed<number>;
                textAlignment: ko.Observable<string> | ko.Computed<string>;
                sizeMode: ko.Observable<string> | ko.Computed<string>;
            }
            class XRCharacterCombSurface extends XRControlSurface {
                private _createCell;
                private _updateCellsText;
                private _roundingTwoDecimals;
                private _getBorderWidthBySpacing;
                private _applyBounds;
                updateArray(cellsCount: number, array?: Array<any>): void;
                fitBoundsToText(): void;
                constructor(control: XRCharacterComb, context: Analytics.Elements.ISurfaceContext);
                getText(): string;
                borderWidth: ko.Computed<number>;
                borders: ko.Observable<string> | ko.Computed<string>;
                verticalSpacing: ko.Observable<number> | ko.Computed<number>;
                horizontalSpacing: ko.Observable<number> | ko.Computed<number>;
                fullCellWidth: ko.Computed<number>;
                fullCellHeight: ko.Computed<number>;
                cellSize: Analytics.Elements.ISize;
                rtl: () => boolean;
                vertical: ko.Computed<number>;
                horizontal: ko.Computed<number>;
                topEmptySpace: ko.Computed<number>;
                leftEmptySpace: ko.Computed<number>;
                cells: ko.ObservableArray<any>;
            }
            module Metadata {
                var cellVerticalSpacing: Analytics.Utils.ISerializationInfo;
                var cellHorizontalSpacing: Analytics.Utils.ISerializationInfo;
                var cellWidth: Analytics.Utils.ISerializationInfo;
                var cellHeight: Analytics.Utils.ISerializationInfo;
                var cellSizeMode: Analytics.Utils.ISerializationInfo;
                var characterCombFont: Analytics.Utils.ISerializationInfo;
                var characterCombBorders: Analytics.Utils.ISerializationInfo;
                var characterCombBorderDashStyle: Analytics.Utils.ISerializationInfo;
                var characterCombSerializationsInfo: Analytics.Utils.ISerializationInfo[];
            }
            class XRChartViewModel extends XRControlViewModel {
                static assignValueDataMembers(chart: Chart.Internal.Models.ChartViewModel, str: string): void;
                static setDataMembers(chart: Chart.Internal.Models.ChartViewModel, isPivotGrid: boolean): void;
                private _createChartModel;
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                isPropertyDisabled(name: string): any;
                getPath(propertyName: string): string;
                pivotGridDataSourceOptions: ko.Computed<any>;
                isPivotGridDataSource: ko.Observable<boolean> | ko.Computed<boolean>;
                seriesDataMember: ko.Observable<string> | ko.Computed<string>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                chart: Chart.Internal.Models.ChartViewModel;
                chartModel: Chart.Internal.ChartControlViewModel;
                dataSource: ko.Observable | ko.Computed;
                realDataSource: ko.Observable | ko.Computed;
            }
            class XRChartSurface extends XRControlSurface {
                constructor(control: XRChartViewModel, context: Analytics.Elements.ISurfaceContext);
                designTime: ko.Observable<boolean>;
                isLoading: ko.Observable<boolean>;
                imageSrc: ko.Observable<string>;
                runDesignerButtonText(): any;
            }
            module Metadata {
                var chart: Analytics.Utils.ISerializationInfo;
                var xrChartSerializationInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class XRCheckBoxViewModel extends XRControlViewModel {
                constructor(control: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                checked: ko.Observable<boolean> | ko.Computed<boolean>;
                checkState: ko.Observable<string> | ko.Computed<string>;
                glyphAlignment: ko.Observable<string> | ko.Computed<string>;
                glyphOptions: GlyphOptions;
            }
            class XRCheckBoxSurface extends XRTextControlSurfaceBase<XRCheckBoxViewModel> {
                constructor(control: XRCheckBoxViewModel, context: Analytics.Elements.ISurfaceContext);
                borderCss: any;
                checkStateClass: ko.Computed<string>;
                checkStateStyleIcon: ko.Computed<string>;
                customGlyphStyleCss: ko.Computed<any>;
                checkStateWidth: ko.Observable<number> | ko.Computed<number>;
                checkStateHeight: ko.Observable<number> | ko.Computed<number>;
                textWidth: ko.Computed<number>;
                leftPadding: any;
                checkStateWidthContainer: ko.Observable<string> | ko.Computed<string>;
                visibleText: ko.Observable<boolean> | ko.Computed<boolean>;
                isGlyphAlignmentNear: ko.Computed<boolean>;
            }
            module Metadata {
                var checkState: Analytics.Utils.ISerializationInfo;
                var checked: Analytics.Utils.ISerializationInfo;
                var glyphOptions: Analytics.Utils.ISerializationInfo;
                var checkEditOptions: Analytics.Utils.ISerializationInfo;
                var checkboxSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesCheckBox: string[];
            }
            class XRCrossBandControlViewModel extends XRReportElementViewModel {
                static unitProperties: string[];
                constructor(control: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                private _subscribeBands;
                getNearestParent(target: Analytics.Elements.IElementViewModel): Analytics.Elements.IElementViewModel;
                isResettableProperty(propertyName: string): boolean;
                isPropertyVisible(name: any): boolean;
                getControlContainerName(): string;
                name: ko.Observable<string> | ko.Computed<string>;
                isCrossbandShow: ko.Computed<boolean>;
                endPoint: Analytics.Elements.Point;
                startPoint: Analytics.Elements.Point;
                locationF: Analytics.Elements.Point;
                startBand: ko.Observable<Bands.BandViewModel>;
                endBand: ko.Observable<Bands.BandViewModel>;
                width: ko.Observable<number> | ko.Computed<number>;
                surface: XRCrossBandSurface;
                parentModel: ko.Observable<ReportViewModel>;
            }
            class XRCrossBandSurface extends XRControlSurfaceBase<XRCrossBandControlViewModel> {
                static _unitProperties: Analytics.Internal.IUnitProperties<XRCrossBandControlViewModel>;
                private _isBandCollapsed;
                private _updateEndPoint;
                private _getAllBands;
                private _getIntersectionBands;
                private _getCrossBandBoxSides;
                constructor(control: XRCrossBandControlViewModel, context: Analytics.Elements.ISurfaceContext);
                isThereIntersectionWithControls(): boolean;
                updateAbsolutePosition(): void;
                isThereIntersectionWithCrossBandControls(): boolean;
                edgeUnderCursor: ko.Observable<Analytics.Internal.IHoverInfo> | ko.Computed<Analytics.Internal.IHoverInfo>;
                underCursor: ko.Observable<Analytics.Internal.IHoverInfo> | ko.Computed<Analytics.Internal.IHoverInfo>;
                leftCss: ko.Observable | ko.Computed;
                visible: ko.Computed<boolean>;
                rightCss: ko.Observable | ko.Computed;
                bottomCss: ko.Observable | ko.Computed;
                topCss: ko.Observable | ko.Computed;
                lineCss: ko.Observable | ko.Computed;
                parent: ReportSurface;
                lineWidthCss: ko.Observable | ko.Computed;
                borderWidth: ko.Computed<number>;
                container(): Analytics.Elements.SurfaceElementArea<Analytics.Elements.ElementViewModel>;
                _getChildrenHolderName(): any;
            }
            module Metadata {
                var crossBandLineWidth: Analytics.Utils.ISerializationInfo;
                var startPoint: Analytics.Utils.ISerializationInfo;
                var endPoint: Analytics.Utils.ISerializationInfo;
                var startBand: Analytics.Utils.ISerializationInfo;
                var endBand: Analytics.Utils.ISerializationInfo;
                var borderDashStyleCrossband: Analytics.Utils.ISerializationInfo;
                var width: Analytics.Utils.ISerializationInfo;
                var crossBandBoxControlSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var crossBandLineControlSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesCrossLine: string[];
            }
            class XRGaugeViewModel extends XRControlViewModel {
                static bindings: string[];
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                viewType: ko.Observable<string> | ko.Computed<string>;
                viewStyle: ko.Observable<string> | ko.Computed<string>;
            }
            module Metadata {
                var circularValues: Array<Analytics.Utils.IDisplayedValue>;
                var linearValues: Array<Analytics.Utils.IDisplayedValue>;
                var actualValue: Analytics.Utils.ISerializationInfo;
                var maximum: Analytics.Utils.ISerializationInfo;
                var minimum: Analytics.Utils.ISerializationInfo;
                var tickmarkCount: Analytics.Utils.ISerializationInfo;
                var targetValue: Analytics.Utils.ISerializationInfo;
                var viewStyle: Analytics.Utils.ISerializationInfo;
                var viewTheme: Analytics.Utils.ISerializationInfo;
                var viewType: Analytics.Utils.ISerializationInfo;
                var xrGaugeSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesGauge: string[];
            }
            class XRLineSurface extends XRControlSurface {
                constructor(control: XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
                linePosition: any;
            }
            module Metadata {
                var lineDirection: Analytics.Utils.ISerializationInfo;
                var lineSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesLine: string[];
            }
            class XRPageBreakSurface extends XRControlSurfaceBase<XRControlViewModel> {
                static _unitProperties: Analytics.Internal.IUnitProperties<XRControlViewModel>;
                preInitProperties(control: any, context: any): void;
                constructor(control: XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
                readonly isIntersectionDeny: boolean;
                linePosition: any;
                lineHeight: ko.Computed<number>;
            }
            module Metadata {
                var pageBreakSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class XRPageInfoSurface extends XRControlSurface {
                constructor(control: XRPageInfoViewModel, context: Analytics.Elements.ISurfaceContext);
            }
            class XRPageInfoViewModel extends XRControlViewModel {
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
            }
            module Metadata {
                var pageInfo: Analytics.Utils.ISerializationInfo;
                var startPageNumber: Analytics.Utils.ISerializationInfo;
                var runningBand: Analytics.Utils.ISerializationInfo;
                var pageInfoSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesPageInfo: string[];
            }
            class XRPictureBoxViewModel extends XRControlViewModel {
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                isAlignmentDisabled(): boolean;
                isPropertyDisabled(propertyName: string): any;
                imageAlignment: ko.Observable<string> | ko.Computed<string>;
                imageUrl: ko.Observable<string> | ko.Computed<string>;
                imageSource: ko.Observable<DevExpress.Reporting.ImageSource>;
                _sizing: ko.Observable<string> | ko.Computed<string>;
                sizing: ko.Observable<string> | ko.Computed<string>;
                isSmallerImage: ko.Observable<boolean> | ko.Computed<boolean>;
                format: ko.Observable<string>;
                readonly isAutoSize: boolean;
            }
            class XRPictureBoxSurface extends XRControlSurface {
                private _createBackgroundPosition;
                private _createBackimage;
                private _createBackgroundOrigin;
                constructor(control: XRPictureBoxViewModel, context: Analytics.Elements.ISurfaceContext);
                getResizeOptions(resizeHandler: DevExpress.Analytics.Internal.IResizeHandler): Analytics.Internal.IResizeHandler;
                selectiontemplate: string;
                getAdornTemplate(): string;
                _control: XRPictureBoxViewModel;
                resizeDisabled: ko.Computed<boolean>;
                resizeOptions: DevExpress.Analytics.Internal.IResizeHandler;
            }
            module Metadata {
                var imageUrl: Analytics.Utils.ISerializationInfo;
                var imageSource: Analytics.Utils.ISerializationInfo;
                var sizing: Analytics.Utils.ISerializationInfo;
                var imageAlignment: Analytics.Utils.ISerializationInfo;
                var imageEditOptions: Analytics.Utils.ISerializationInfo;
                var pictureBoxSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesPicture: string[];
            }
            class XRPivotGridViewModel extends XRControlViewModel {
                dispose(): void;
                private _initCriteriaString;
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                removeChild(selectedField: PivotGrid.PivotGridFieldViewModel): void;
                getFieldsFromArea(area: string): PivotGrid.PivotGridFieldViewModel[];
                getPath(propertyName: any): string;
                fields: ko.ObservableArray<PivotGrid.PivotGridFieldViewModel>;
                dataSource: ko.Observable<Data.ObjectStorageItem>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                addFieldToArea: any;
                prefilter: {
                    parent: XRPivotGridViewModel;
                    _criteriaString: ko.Observable<string> | ko.Computed<string>;
                    criteriaString: DevExpress.Analytics.Widgets.FilterStringOptions;
                };
            }
            class XRPivotGridSurface extends XRControlSurface {
                constructor(control: XRPivotGridViewModel, context: Analytics.Elements.ISurfaceContext);
                _getChildrenHolderName(): string;
                getAreaFields(area: string): PivotGrid.PivotGridFieldSurface[];
                getTotalsAreaFieldWidth(area: string, zoom: number): number;
                getAdornTemplate(): "" | "dxrd-intersect";
                isThereIntersectionWithChildCollection(): boolean;
                filterFields: ko.Computed<PivotGrid.PivotGridFieldSurface[]>;
                dataFields: ko.Computed<PivotGrid.PivotGridFieldSurface[]>;
                columnFields: ko.Computed<PivotGrid.PivotGridFieldSurface[]>;
                rowFields: ko.Computed<PivotGrid.PivotGridFieldSurface[]>;
                totalsHeight: ko.Computed<number>;
                rowHeaderHeight: ko.Computed<number>;
                totalsDataFieldWidth: ko.Computed<number>;
                totalsRowFieldWidth: ko.Computed<number>;
                fields: ko.ObservableArray<PivotGrid.PivotGridFieldSurface>;
            }
            module Metadata {
                var pivotGridAppearances: Analytics.Utils.ISerializationInfo;
                var prefilter: Analytics.Utils.ISerializationInfo;
                var pivotGridOptions: Analytics.Utils.ISerializationInfoArray;
                var pivotGridSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class ReportViewModel extends XRReportElementViewModel implements Analytics.Utils.IModelReady {
                static availableDataSourceTypes: string[];
                static bandsTypeOrdering: string[];
                static unitProperties: string[];
                static defaultPageSize: {
                    width: number;
                    height: number;
                };
                private _getDpi;
                private _recalculateUnits;
                findStyle(styleName: any): any;
                _initializeBands(): void;
                isPropertyDisabled(name: string): boolean;
                getSubreportUrls(): void;
                dispose(): void;
                preInitProperties(): void;
                constructor(report: any, serializer?: Analytics.Utils.IModelSerializer);
                initialize(): void;
                getNearestParent(target: any): this;
                addChild(control: Analytics.Elements.IElementViewModel): void;
                removeChild(control: Analytics.Elements.ElementViewModel): void;
                serialize(): any;
                save(): any;
                getPath(propertyName: string): string;
                clone(): ReportViewModel;
                paperKind: ko.Observable<string> | ko.Computed<string>;
                isStyleProperty(propertyName: string): boolean;
                onSave: (data: any) => void;
                dataSourceHelper: ko.Observable<Internal.DataSourceHelper>;
                dataSource: ko.Observable<Data.ObjectStorageItem>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                styles: ko.ObservableArray<StyleModel>;
                measureUnit: ko.Observable<string> | ko.Computed<string>;
                snapGridSize: ko.Observable<number> | ko.Computed<number>;
                pageWidth: ko.Observable<number> | ko.Computed<number>;
                pageHeight: ko.Observable<number> | ko.Computed<number>;
                margins: Analytics.Elements.Margins;
                bands: ko.ObservableArray<Bands.BandViewModel>;
                crossBandControls: ko.ObservableArray<XRCrossBandControlViewModel>;
                parameters: ko.ObservableArray<Data.Parameter>;
                parameterHelper: ReportParameterHelper;
                objectsStorageHelper: Data.ObjectsStorage;
                objectStorage: ko.ObservableArray<Data.ObjectStorageItem>;
                componentStorage: ko.ObservableArray<Data.ObjectStorageItem>;
                _dataBindingMode: ko.Observable<string> | ko.Computed<string>;
                readonly dataBindingMode: string;
                extensions: ko.ObservableArray<ExtensionModel>;
                formattingRuleSheet: ko.ObservableArray<FormattingRule>;
                components: ko.ObservableArray<ComponentsModel>;
                calculatedFields: ko.ObservableArray<Data.CalculatedField>;
                scriptsSource: ko.Observable<string> | ko.Computed<string>;
                scriptLanguage: ko.Observable<string> | ko.Computed<string>;
                private _getReportUnit;
                private _update;
                surface: ReportSurface;
                exportOptions: Export.ExportOptions;
                isModelReady: ko.Computed<boolean>;
                scriptReferencesString: ko.Computed<string>;
                landscape: ko.Observable<boolean> | ko.Computed<boolean>;
                _scriptReferencesString: ko.Observable<string> | ko.Computed<string>;
                key: ko.Computed<string>;
                dataSourceRefs: Array<Utils.IDataSourceRefInfo>;
                rtlLayout: ko.Observable<string> | ko.Computed<string>;
                drawWatermark: ko.Observable<boolean> | ko.Computed<boolean>;
                watermark: Internal.IReportWatermark;
                displayNameObject: ko.Observable<string>;
            }
            class ReportSurface extends Analytics.Elements.SurfaceElementArea<ReportViewModel> implements Analytics.Internal.ISelectionTarget, Analytics.Elements.ISurfaceContext {
                static _unitProperties: Analytics.Internal.IUnitProperties<ReportViewModel>;
                private _createMargin;
                private _updateWatermarkImageNaturalSize;
                dispose(): void;
                constructor(report: ReportViewModel, zoom?: ko.Observable<number>);
                getChildrenCollection(): ko.ObservableArray<Bands.BandSurface>;
                isFit(dropTarget: Analytics.Internal.ISelectionTarget): boolean;
                canDrop(): boolean;
                wrapRtlProperty(data: {
                    value: ko.Observable | ko.Computed;
                }, undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>, element: Element): any;
                allowMultiselect: boolean;
                locked: boolean;
                focused: ko.Observable<boolean>;
                selected: ko.Observable<boolean>;
                templateName: ko.Observable<string>;
                bandsHolder: Bands.Internal.BandsHolder;
                underCursor: ko.Observable<Analytics.Internal.IHoverInfo>;
                crossBandControls: ko.ObservableArray<XRCrossBandSurface>;
                measureUnit: ko.Observable<string> | ko.Computed<string>;
                zoom: ko.Observable<number> | ko.Computed<number>;
                margins: Analytics.Elements.IMargins;
                dpi: ko.Observable<number> | ko.Computed<number>;
                rtl: ko.Observable<boolean> | ko.Computed<boolean>;
                pageWidthWithoutMargins: ko.Computed<number>;
                ghostContainerOffset: ko.Computed<number>;
                checkParent(surfaceParent: Analytics.Internal.ISelectionTarget): boolean;
                effectiveHeight: ko.Computed<number>;
                minHeight: ko.Observable<number>;
                maxMarkerWidth: ko.Observable<number>;
                pageWidth: ko.Observable<number> | ko.Computed<number>;
                pageHeight: ko.Observable<number> | ko.Computed<number>;
                validationMode: ko.Observable<boolean>;
                parent: Analytics.Internal.ISelectionTarget;
                leftMarginOffset: ko.Computed<number>;
                rightMarginOffset: ko.Computed<number>;
                rightMarginResizableOffset: ko.Computed<number>;
                rightMarginResizeOptions: (undoEngine: ko.Observable<Analytics.Utils.UndoEngine>, element: Element) => {};
                leftMarginResizeOptions: (undoEngine: ko.Observable<Analytics.Utils.UndoEngine>, element: Element) => {};
                leftMarginResizableOffset: ko.Computed<number>;
                drawWatermark: ko.Observable<boolean> | ko.Computed<boolean>;
                watermark: Internal.IReportWatermark;
                _watermarkImageNaturalSize: ko.Observable<{
                    width: number;
                    height: number;
                }>;
                _watermarkTextRenderingResult: ko.Observable<string> | ko.Computed<string>;
            }
            module Metadata {
                var paperKind: Analytics.Utils.ISerializationInfo;
                var landscape: Analytics.Utils.ISerializationInfo;
                var margins: Analytics.Utils.ISerializationInfo;
                var pageColor: Analytics.Utils.ISerializationInfo;
                var measureUnit: Analytics.Utils.ISerializationInfo;
                var snapGridSize: Analytics.Utils.ISerializationInfo;
                var drawWatermark: Analytics.Utils.ISerializationInfo;
                var showPreviewMarginLines: Analytics.Utils.ISerializationInfo;
                var verticalContentSplitting: Analytics.Utils.ISerializationInfo;
                var reportExportOptionsSerializationInfo: Analytics.Utils.ISerializationInfo;
                var foreColorWatermark: Analytics.Utils.ISerializationInfo;
                var fontWatermark: Analytics.Utils.ISerializationInfo;
                var watermarkSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var watermark: Analytics.Utils.ISerializationInfo;
                var rollPaper: Analytics.Utils.ISerializationInfo;
                var requestParameters: Analytics.Utils.ISerializationInfo;
                var formattingRuleSheet: Analytics.Utils.ISerializationInfo;
                var pageWidth: Analytics.Utils.ISerializationInfo;
                var pageHeight: Analytics.Utils.ISerializationInfo;
                var scriptLanguage: Analytics.Utils.ISerializationInfo;
                var scriptReferencesString: Analytics.Utils.ISerializationInfo;
                var calculatedFields: Analytics.Utils.ISerializationInfo;
                var parametersInfo: Analytics.Utils.ISerializationInfo;
                var bookmarkDuplicateSuppress: Analytics.Utils.ISerializationInfo;
                var horizontalContentSplitting: Analytics.Utils.ISerializationInfo;
                var rtlLayout: Analytics.Utils.ISerializationInfo;
                var rtlReport: Analytics.Utils.ISerializationInfo;
                var reportSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesReport: string[];
            }
            enum XRRichTextStreamType {
                RtfText = 0,
                PlainText = 1,
                HtmlText = 2,
                XmlText = 3
            }
            class XRRichViewModel extends XRControlViewModel {
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                readonly textEditableProperty: ko.Observable<string>;
                format: ko.Observable<XRRichTextStreamType>;
                foreColor: ko.Observable<string> | ko.Computed<string>;
                _serializableRtfString: ko.Observable<string> | ko.Computed<string>;
                font: ko.Observable<string> | ko.Computed<string>;
                textRtf: ko.Observable<string>;
                _rtf: ko.Observable<string>;
            }
            class XRRichSurface extends XRControlSurface {
                private _lastRequest;
                private _innerUpdate;
                private _sendCallback;
                constructor(control: XRRichViewModel, context: Analytics.Elements.ISurfaceContext);
                imageSrc: ko.Observable<string>;
                isLoading: ko.Observable<boolean>;
            }
            module Metadata {
                var rtf: Analytics.Utils.ISerializationInfo;
                var textRtf: Analytics.Utils.ISerializationInfo;
                var serializableRtfString: Analytics.Utils.ISerializationInfo;
                var richTextSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesRichText: string[];
            }
            class XRShapeViewModel extends XRControlViewModel {
                static timeout: number;
                static shapes: ({
                    displayName: string;
                    type: string;
                    angle?: undefined;
                    val?: undefined;
                } | {
                    displayName: string;
                    type?: undefined;
                    angle?: undefined;
                    val?: undefined;
                } | {
                    displayName: string;
                    angle: number;
                    type: string;
                    val?: undefined;
                } | {
                    displayName: string;
                    val: {
                        "@NumberOfSides": number;
                        "@StarPointCount"?: undefined;
                    };
                    type: string;
                    angle?: undefined;
                } | {
                    displayName: string;
                    val: {
                        "@StarPointCount": number;
                        "@NumberOfSides"?: undefined;
                    };
                    type: string;
                    angle?: undefined;
                })[];
                static createShape(model: any, serializer?: any): {
                    "shapeType": ko.Observable<any>;
                    "getInfo": () => any;
                };
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                Shape: any;
                shapeFake: any;
            }
            class XRShapeControlSurface extends XRControlSurface {
                constructor(control: XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
                imageSrc: ko.Computed<string>;
            }
            module Metadata {
                var shapeType: Analytics.Utils.ISerializationInfo;
                var stretch: Analytics.Utils.ISerializationInfo;
                var fillColor: Analytics.Utils.ISerializationInfo;
                var Shape: Analytics.Utils.ISerializationInfo;
                var shapeFake: Analytics.Utils.ISerializationInfo;
                var shapeElementSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var shapesMap: {
                    "Rectangle": Analytics.Utils.ISerializationInfoArray;
                    "Arrow": Analytics.Utils.ISerializationInfoArray;
                    "Ellipse": Analytics.Utils.ISerializationInfoArray;
                    "Polygon": Analytics.Utils.ISerializationInfoArray;
                    "Star": Analytics.Utils.ISerializationInfoArray;
                    "Line": Analytics.Utils.ISerializationInfoArray;
                    "Bracket": Analytics.Utils.ISerializationInfoArray;
                    "Cross": Analytics.Utils.ISerializationInfoArray;
                    "Brace": Analytics.Utils.ISerializationInfoArray;
                };
                var shapeSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesShape: string[];
            }
            class XRSparklineViewModel extends XRControlViewModel {
                static spartlineTypes: {
                    value: string;
                    displayValue: string;
                    localizationId: string;
                }[];
                createView(model: any, serializer?: any): {
                    "type": ko.Observable<any>;
                    "getInfo": () => any;
                };
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                getPath(propertyName: any): any;
                view: any;
                dataSource: ko.Observable<any>;
                dataMember: ko.Observable<string> | ko.Computed<string>;
                sparklineFake: any;
                valueMember: ko.Observable<string> | ko.Computed<string>;
            }
            class XRSparkLineSurface extends Internal.TodoControlSurface {
                constructor(control: XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
            }
            module Metadata {
                var valueMember: Analytics.Utils.ISerializationInfo;
                var sparklineViewMap: {
                    "Line": Analytics.Utils.ISerializationInfoArray;
                    "Bar": Analytics.Utils.ISerializationInfoArray;
                    "WinLoss": Analytics.Utils.ISerializationInfoArray;
                    "Area": Analytics.Utils.ISerializationInfoArray;
                };
                var valueRange: Analytics.Utils.ISerializationInfo;
                var sparklineFake: Analytics.Utils.ISerializationInfo;
                var sparklineSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesSparkline: string[];
            }
            class SubreportViewModel extends ReportViewModel {
                static defaultReport: {
                    "@ControlType": string;
                    "@PageWidth": string;
                    "@PageHeight": string;
                    "@Version": string;
                    "@Font": string;
                    "@Dpi": string;
                    "Bands": {
                        "Item1": {
                            "@ControlType": string;
                            "@HeightF": string;
                        };
                        "Item2": {
                            "@ControlType": string;
                            "@HeightF": string;
                        };
                        "Item3": {
                            "@ControlType": string;
                            "@HeightF": string;
                        };
                    };
                };
                _initializeBands(): void;
                static from(model: any, serializer?: Analytics.Utils.IModelSerializer): SubreportViewModel;
                static toJson(value: SubreportViewModel, serializer: any, refs: any): any;
                getInfo(): any;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                serialize(): any;
                isAllSufficient: boolean;
            }
            class ParameterBinding extends Data.DataBindingBase implements DevExpress.Analytics.Utils.ISerializableModel {
                private _reportDataSource;
                static createNew(): ParameterBinding;
                dispose(): void;
                getInfo(): any;
                updateParameter(pathRequest: Analytics.Utils.PathRequest, dataSources: any): void;
                refresh(): void;
                constructor(model: any, parent: any, serializer?: Analytics.Utils.IModelSerializer);
                visible: ko.Observable<boolean>;
                parameterName: ko.Observable<string> | ko.Computed<string>;
                subreportControl: ko.Observable<XRSubreportViewModel> | ko.Computed<XRSubreportViewModel>;
                fakeBinding: any;
            }
            class XRSubreportViewModel extends XRControlViewModel {
                dispose(): void;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                static _patchModel(model: any): any;
                private _getCurrentGenerateOwnPagesIsActive;
                private _calculateSubreportPosition;
                constructor(model: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.IModelSerializer);
                refreshParameterBindings(): void;
                isPropertyDisabled(propertyName: any): any;
                updateParameters(): void;
                cloneReportSource(): ReportViewModel;
                needProcessLocation: boolean;
                root: ReportViewModel;
                generateOwnPages: ko.Computed<boolean>;
                _generateOwnPages: ko.Observable<boolean> | ko.Computed<boolean>;
                generateOwnPagesIsActive: ko.Computed<boolean>;
                subreportParameters: ko.ObservableArray<{
                    name: string;
                    displayName: string;
                }>;
                reportSource: ReportViewModel;
                reportSourceUrl: ko.Observable<string> | ko.Computed<string>;
                parameterBindings: ko.ObservableArray<ParameterBinding>;
                key: ko.Computed<string>;
            }
            class XRSubreportSurface extends XRControlSurface {
                constructor(control: XRSubreportViewModel, context: Analytics.Elements.ISurfaceContext);
                getAdornTemplate(): "" | "dxrd-intersect";
                getResizableOptions(resizeHandler: any): any;
                processLocation(location: Analytics.Elements.IArea): Analytics.Elements.IArea;
                _control: XRSubreportViewModel;
            }
            module Metadata {
                var reportSourceUrl: Analytics.Utils.ISerializationInfo;
                var reportSource: Analytics.Utils.ISerializationInfo;
                var parameterBindingSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var parameterBindings: Analytics.Utils.ISerializationInfo;
                var generateOwnPages: Analytics.Utils.ISerializationInfo;
                var subreportSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class XRTableControlViewModel extends XRControlViewModel {
                private _getAdjacentCells;
                dispose(): void;
                constructor(control: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                updateRowLocation(row: XRTableRowViewModel, deltaHeight: number): void;
                addChild(control: Analytics.Elements.IElementViewModel, position?: number, onComponentAdded?: any): void;
                insertRow(selectedRow: XRTableRowViewModel, isRowAbove: boolean, onComponentAdded: any): void;
                removeChild(selectedRow: XRTableRowViewModel): void;
                insertColumn(selectedCell: XRTableCellViewModel, isRight: boolean, onComponentAdded: any): void;
                addColumnToCalculation(diff: number, last?: boolean): void;
                tableCalculationProvider: Internal.TableCalculationProvider;
                rows: ko.ObservableArray<XRTableRowViewModel>;
                rowsTotalWeight: ko.Computed<number>;
                pixelHeightWeight: ko.Computed<number>;
                name: ko.Observable<string> | ko.Computed<string>;
                size: Analytics.Elements.Size;
                location: Analytics.Elements.Point;
                surface: XRTableSurface;
            }
            class XRTableSurface extends XRControlSurfaceBase<XRTableControlViewModel> {
                private _isUpdating;
                static _unitProperties: Analytics.Internal.IUnitProperties<XRTableControlViewModel>;
                _getChildrenHolderName(): string;
                dispose(): void;
                constructor(control: XRTableControlViewModel, context: Analytics.Elements.ISurfaceContext);
                private _isCellInColumn;
                selectColumn(selection: Analytics.Internal.ISelectionProvider, cellSurface: XRTableCellSurface): void;
                isThereIntersectionWithChildCollection(): boolean;
                rows: ko.ObservableArray<XRTableRowSurface>;
            }
            module Metadata {
                var processHiddenCellMode: Analytics.Utils.ISerializationInfo;
                var tableSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesTable: string[];
            }
            class XRTableCellViewModel extends XRControlViewModel {
                static unitProperties: any[];
                constructor(model: any, parent: XRTableRowViewModel, serializer?: Analytics.Utils.ModelSerializer);
                weight: ko.Observable<number> | ko.Computed<number>;
                width: ko.Computed<number>;
                height: ko.Computed<number>;
                left: ko.Observable<number> | ko.Computed<number>;
                name: ko.Observable<string> | ko.Computed<string>;
                text: ko.Observable<string> | ko.Computed<string>;
                surface: XRTableCellSurface;
                borders: ko.Observable<string> | ko.Computed<string>;
                parentModel: ko.Observable<XRTableRowViewModel>;
            }
            class XRTableCellSurface extends Internal.TableComponentSurface<XRTableCellViewModel> {
                private _row;
                private _table;
                private _cellIndex;
                private _rowIndex;
                private _getAdjacentCellByRowIndex;
                private _isShowBorder;
                static _unitProperties: Analytics.Internal.IUnitProperties<XRTableCellViewModel>;
                dispose(): void;
                constructor(control: XRTableCellViewModel, context: Analytics.Elements.ISurfaceContext);
                direction: Internal.TableActionDirection;
                controls: ko.ObservableArray<XRControlSurface>;
                x: ko.Observable<number> | ko.Computed<number>;
                rowSpan: ko.Computed<number>;
                heightWithRowSpan: () => number;
                offsetZIndex: () => number;
                selectColumn(selection: Analytics.Internal.ISelectionProvider): void;
                checkParent(surfaceParent: Analytics.Internal.ISelectionTarget): boolean;
                isThereIntersectionWithUsefulArea(): boolean;
                isThereIntersectionWithCrossBandControls(): boolean;
                isThereIntersectionWithNeighborsCollection(): boolean;
                isThereIntersectionWithParentCollection(): boolean;
                beforeRectUpdated(rect: Analytics.Elements.IArea): Analytics.Elements.IArea;
                canDrop(): boolean;
            }
            module Metadata {
                var weight: Analytics.Utils.ISerializationInfo;
                var rowSpan: Analytics.Utils.ISerializationInfo;
                var tableCellSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesTableCell: string[];
            }
            class TableOfContentsLevel extends Analytics.Elements.ElementViewModel {
                dispose(): void;
                static createNew(parent: XRTableOfContentsViewModel): TableOfContentsLevel;
                static unitProperties: string[];
                private _levelIndex;
                private _indentFactor;
                private _createPaddingDependencies;
                preInitProperties(model: any): void;
                constructor(model: any, parent: XRTableOfContentsViewModel, serializer?: Analytics.Utils.ModelSerializer, isTitle?: boolean);
                isTitle: boolean;
                name: ko.Observable<string> | ko.Computed<string>;
                height: ko.Observable<number> | ko.Computed<number>;
                width: ko.Observable<number> | ko.Computed<number>;
                top: ko.Observable<number> | ko.Computed<number>;
                levelsHeightUnder: ko.Observable<number> | ko.Computed<number>;
                indent: ko.Observable<number> | ko.Computed<number>;
                left: ko.Computed<number>;
                leaderSymbol: ko.Observable<string> | ko.Computed<string>;
                font: ko.Observable<string> | ko.Computed<string>;
                foreColor: ko.Observable<string> | ko.Computed<string>;
                backColor: ko.Observable<string> | ko.Computed<string>;
                padding: ko.Observable<string> | ko.Computed<string>;
                paddingObj: Analytics.Elements.PaddingModel;
                textAlignment: ko.Observable<string> | ko.Computed<string>;
                text: ko.Observable<string> | ko.Computed<string>;
                dpi: ko.Observable<number> | ko.Computed<number>;
                parentModel: ko.Observable<XRTableOfContentsViewModel>;
                borderWidth: ko.Observable | ko.Computed;
                borderColor: ko.Observable | ko.Computed;
                borders: ko.Observable | ko.Computed;
                borderDefault: ko.Observable<string> | ko.Computed<string>;
                borderDashStyle: ko.Observable | ko.Computed;
                lockedInUserDesigner: ko.Observable<boolean> | ko.Computed<boolean>;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                isPropertyModified(name: string): boolean;
                getControlFactory(): ControlsFactory;
                rtl(): boolean;
            }
            class TableOfContentsLevelSurface extends XRControlSurfaceBase<TableOfContentsLevel> {
                static _$leaderSymbol: JQuery;
                static _unitProperties: Analytics.Internal.IUnitProperties<TableOfContentsLevel>;
                private _leaderSymbolWidth;
                constructor(control: TableOfContentsLevel, context: Analytics.Elements.ISurfaceContext);
                text: ko.Observable<string> | ko.Computed<string>;
                resizable(resizeHandler: any, element: HTMLElement): any;
                leaderSymbols: ko.PureComputed<string>;
                rtlLayout(): boolean;
            }
            module Metadata {
                var baseTocLevelSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var tocLevelSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class XRTableOfContentsViewModel extends XRControlViewModel {
                dispose(): void;
                constructor(control: any, parent: Bands.BandViewModel, serializer?: Analytics.Utils.ModelSerializer);
                readonly textEditableProperty: ko.Observable<string> | ko.Computed<string>;
                levels: ko.ObservableArray<TableOfContentsLevel>;
                levelDefault: TableOfContentsLevel;
                levelTitle: TableOfContentsLevel;
                maxNestingLevel: ko.Observable<number> | ko.Computed<number>;
                levelTitleText: ko.Observable<string> | ko.Computed<string>;
                allLevels: ko.Observable<TableOfContentsLevel[]> | ko.Computed<TableOfContentsLevel[]>;
                borderWidth: ko.Observable | ko.Computed;
                borderColor: ko.Observable | ko.Computed;
                borders: ko.Observable | ko.Computed;
                borderDashStyle: ko.Observable | ko.Computed;
                borderDefault: ko.PureComputed<string>;
                parentModel: ko.Observable<Bands.BandViewModel>;
            }
            class XRTableOfContentsSurface extends XRControlSurface {
                constructor(control: XRTableOfContentsViewModel, context: Analytics.Elements.ISurfaceContext);
                isThereIntersectionWithChildCollection(): boolean;
                isThereIntersectionWithUsefulArea(): boolean;
                isThereIntersectionWithParentCollection(): boolean;
                levelTitle: TableOfContentsLevelSurface;
                levelDefault: TableOfContentsLevelSurface;
                levels: ko.ObservableArray<TableOfContentsLevelSurface>;
            }
            module Metadata {
                var tocTitleSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
                var tocTitle: Analytics.Utils.ISerializationInfo;
                var tocLevelDefault: Analytics.Utils.ISerializationInfo;
                var maxNestingLevel: Analytics.Utils.ISerializationInfo;
                var tocLevels: Analytics.Utils.ISerializationInfo;
                var tocSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class XRTableRowViewModel extends XRControlViewModel {
                static unitProperties: any[];
                dispose(): void;
                constructor(control: any, parent: XRTableControlViewModel, serializer?: Analytics.Utils.ModelSerializer);
                hasCalculationNode(cellIndex: number): boolean;
                addCellToCalculation(cellIndex: number, delta: number): void;
                addColumnToCalculation(diff: number, last?: boolean): void;
                addTableOffset(width?: number, left?: number): void;
                addChild(control: Analytics.Elements.IElementViewModel, position?: number, onComponentAdded?: any): void;
                insertCellCopy(selectedCell: XRTableCellViewModel, isRight: boolean, onComponentAdded: any): void;
                removeChild(selectedCell: XRTableCellViewModel): void;
                parentModel: ko.Observable<XRTableControlViewModel>;
                cells: ko.ObservableArray<XRTableCellViewModel>;
                cellsTotalWeight: ko.Computed<number>;
                pixelWidthWeight: ko.Computed<number>;
                name: ko.Observable<string> | ko.Computed<string>;
                weight: ko.Observable<number> | ko.Computed<number>;
                width: ko.Observable<number> | ko.Computed<number>;
                height: ko.Computed<number>;
                top: ko.Observable<number> | ko.Computed<number>;
                surface: XRTableRowSurface;
            }
            class XRTableRowSurface extends Internal.TableComponentSurface<XRTableRowViewModel> {
                static _unitProperties: Analytics.Internal.IUnitProperties<XRTableRowViewModel>;
                _getChildrenHolderName(): string;
                dispose(): void;
                constructor(control: XRTableRowViewModel, context: Analytics.Elements.ISurfaceContext);
                getAdornTemplate(): string;
                direction: Internal.TableActionDirection;
                y: ko.Observable<number> | ko.Computed<number>;
                cells: ko.ObservableArray<XRTableCellSurface>;
            }
            module Metadata {
                var tableRowSerializationsInfo: Analytics.Utils.ISerializationInfoArray;
            }
            class XRZipCodeSurface extends XRControlSurface {
                constructor(control: XRControlViewModel, context: Analytics.Elements.ISurfaceContext);
                fontSize: ko.Computed<number>;
                letterSpacing: ko.Computed<number>;
            }
            module Metadata {
                var segmentWidth: Analytics.Utils.ISerializationInfo;
                var zipCodeSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesZipCode: string[];
            }
        }
        module Bands {
            module Internal {
                var bandsWeight: {
                    "TopMarginBand": number;
                    "ReportHeaderBand": number;
                    "PageHeaderBand": number;
                    "GroupHeaderBand": number;
                    "DetailBand": number;
                    "VerticalHeaderBand": number;
                    "VerticalDetailBand": number;
                    "VerticalTotalBand": number;
                    "DetailReportBand": number;
                    "GroupFooterBand": number;
                    "ReportFooterBand": number;
                    "PageFooterBand": number;
                    "BottomMarginBand": number;
                };
            }
            class GroupFieldModel extends Analytics.Utils.Disposable implements DevExpress.Analytics.Utils.ISerializableModel {
                static createNew: () => GroupFieldModel;
                getInfo(): Analytics.Utils.ISerializationInfoArray;
                constructor(model: any, serializer?: Analytics.Utils.IModelSerializer);
                sortOrder: ko.Observable<string> | ko.Computed<string>;
                sortOrderClass: ko.Computed<{
                    class: string;
                    template: string;
                }>;
                changeSortOrder: () => void;
                fieldName: ko.Observable<string> | ko.Computed<string>;
            }
            module Metadata {
                var groupFields: Analytics.Utils.ISerializationInfo;
                var sortFields: Analytics.Utils.ISerializationInfo;
            }
            class MultiColumn extends Analytics.Elements.SerializableModel {
                static unitProperties: string[];
                constructor(model: any, pageWidth: ko.Observable<number> | ko.Computed<number>, margins: Analytics.Elements.Margins, serializer?: Analytics.Utils.IModelSerializer);
                mode: ko.Observable<string> | ko.Computed<string>;
                realColumnWidth: ko.Observable<number> | ko.Computed<number>;
                grayAreaWidth: ko.Observable<number>;
                columnWidth: ko.Observable<number> | ko.Computed<number>;
                columnCount: ko.Observable<number> | ko.Computed<number>;
                columnSpacing: any;
            }
            module Metadata {
                var multiColumn: Analytics.Utils.ISerializationInfo;
            }
            class MultiColumnSurface extends Analytics.Utils.Disposable {
                constructor(multiColumn: MultiColumn, context: Analytics.Elements.ISurfaceContext);
                grayAreaWidth: ko.Computed<number>;
                columnWidth: ko.Computed<number>;
                columnSpacing: ko.Computed<number>;
                columnSpacingLeft: ko.Computed<number>;
                grayAreaLeft: ko.Computed<number>;
                haveColumns: ko.Computed<boolean>;
            }
            module Internal {
                interface IBandsHolder {
                    bands: ko.ObservableArray<BandSurface>;
                    verticalBandsContainer?: VerticalBandsContainerSurface;
                }
                class BandsHolder extends Analytics.Utils.Disposable implements IBandsHolder {
                    private _container;
                    dispose(): void;
                    private _createBandsMapCollection;
                    private _addHorizontalBand;
                    private _addVerticalBand;
                    initialize(bands: any): void;
                    constructor(_container: Controls.ReportSurface | BandSurface);
                    getHeight(): number;
                    getTotalHeight(): number;
                    getBandAbsolutePositionY(band: BandSurface): number;
                    checkUnderCursor(): boolean;
                    bands: ko.ObservableArray<BandSurface>;
                    verticalBandsContainer: VerticalBandsContainerSurface;
                    multiColumn: ko.Computed<MultiColumnSurface>;
                }
            }
            module Metadata {
                var drillDownDetailReportExpanded: Analytics.Utils.ISerializationInfo, drillDownControl: Analytics.Utils.ISerializationInfo;
                var printAtBottom: Analytics.Utils.ISerializationInfo;
                var level: Analytics.Utils.ISerializationInfo;
                var repeatEveryPage: Analytics.Utils.ISerializationInfo;
                var pageBreak: Analytics.Utils.ISerializationInfo;
                var keepTogetherWithDetailReports: Analytics.Utils.ISerializationInfo;
                var height: Analytics.Utils.ISerializationInfo;
            }
            class BandViewModel extends Controls.XRReportElementViewModel {
                dispose(): void;
                static getBandWeight: (band: BandViewModel) => any;
                static initLevels(bands: BandViewModel[]): void;
                static generateArray(allbands: BandViewModel[], controlType: string, newLevel?: number): any[];
                static replaceArrays(newArray: BandViewModel[], target: ko.ObservableArray<BandViewModel>, band: BandViewModel): void;
                createChildsArray(band: any, serializer: Analytics.Utils.ModelSerializer): void;
                initHeight(): void;
                preInit(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer): void;
                constructor(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                addChild(control: Analytics.Elements.IElementViewModel): void;
                getPath(propertyName: any): any;
                initSize(): void;
                initialize(): void;
                removeChild(control: Analytics.Elements.ElementViewModel): void;
                static isReorderingBand(control: Analytics.Elements.ElementViewModel): boolean;
                static insertBand(bands: ko.ObservableArray<BandViewModel>, newBand: BandViewModel): void;
                static insertBandSorted(bands: ko.ObservableArray<BandViewModel>, newBand: BandViewModel): void;
                isPropertyDisabled(name: string): boolean;
                level: ko.Observable<number> | ko.Computed<number>;
                _level: ko.Observable<number> | ko.Computed<number>;
                size: Analytics.Elements.Size;
                name: ko.Observable<string> | ko.Computed<string>;
                height: ko.Observable<number> | ko.Computed<number>;
                bands: ko.ObservableArray<BandViewModel>;
                controls: ko.ObservableArray<Controls.XRControlViewModel>;
                heightFromControls: ko.Computed<number>;
                expanded: ko.Observable<boolean> | ko.Computed<boolean>;
                parentModel: ko.Observable<BandViewModel | Controls.ReportViewModel>;
            }
            class BandSurface extends Analytics.Elements.SurfaceElementBase<BandViewModel> {
                static collapsedHeight: number;
                private _getMarginWidth;
                coordinateGridOptions: any;
                dispose(): void;
                static _unitProperties: Analytics.Internal.IUnitProperties<BandViewModel>;
                isSomeParentCollapsed: ko.Observable<boolean>;
                private _resize;
                createChildCollection(band: BandViewModel): void;
                createUnderCursor(): void;
                getTotalHeight(): number;
                getHeight(): number;
                getHasOwnRuler(): boolean;
                getBackgroundRect(): {
                    top: number;
                    bottom: any;
                    height: number;
                };
                protected _initMultiColumn(): void;
                constructor(band: BandViewModel, context: Analytics.Elements.ISurfaceContext, unitProperties?: Analytics.Internal.IUnitProperties<BandViewModel>);
                getAbsolutePositionY(): number;
                updateAbsolutePosition(): void;
                markerClick(selection: Analytics.Internal.SurfaceSelection): void;
                showMarker: boolean;
                templateName: string;
                selectionTemplate: string;
                vrulerTemplate: string;
                contentSelectionTemplate: string;
                leftMarginTemplate: string;
                leftMarginSelectionTemplate: string;
                canDrop(): boolean;
                minHeight: ko.Computed<number>;
                allowMultiselect: boolean;
                heightFromControls: ko.Computed<number>;
                subBandsHeight: ko.Computed<number>;
                heightWithoutSubBands: ko.Computed<number>;
                hasOwnRuler: ko.Computed<boolean>;
                rulerHeight: ko.Computed<number>;
                height: ko.Computed<number>;
                markerWidth: ko.Observable<number>;
                name: ko.Observable<string> | ko.Computed<string>;
                parent: Controls.ReportSurface | BandSurface;
                bandsHolder: Internal.BandsHolder;
                controls: ko.ObservableArray<Controls.XRControlSurface>;
                readonly zoom: ko.Observable<number> | ko.Computed<number>;
                collapsed: ko.Observable<boolean> | ko.Computed<boolean>;
                checkParent(surfaceParent: Controls.XRControlSurfaceBase<any>): boolean;
                canResize: ko.Computed<boolean>;
                backgroundRect: ko.Computed<Analytics.Elements.IArea>;
                _totalHeight: ko.Computed<number>;
                multiColumn: ko.Computed<MultiColumnSurface>;
            }
            module Metadata {
                var expanded: Analytics.Utils.ISerializationInfo;
                var commonBandSerializationInfo: DevExpress.Analytics.Utils.ISerializationInfoArray;
                var bandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var reportHeaderBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var reportFooterBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesReportHeader: string[];
                var popularPropertiesReportFooter: string[];
            }
            interface IHierarchyPrintOptions {
                keyFieldName: ko.Observable<string>;
                childListFieldName: ko.Observable<string>;
                parentFieldName: ko.Observable<string>;
                indent: ko.Observable<number>;
                keepTogetherWithFirstChild: ko.Observable<boolean>;
                isPropertyDisabled: (propertyName: string) => boolean;
                getPath: (propertyName?: string) => string;
            }
            class DetailBand extends BandViewModel {
                dispose(): void;
                preInit(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer): void;
                hasHierarchyPrintOptions(): boolean;
                constructor(band: any, parent: any, serializer: any);
                multiColumn: MultiColumn;
                hierarchyPrintOptions: IHierarchyPrintOptions;
                sortFields: ko.ObservableArray<GroupFieldModel>;
            }
            class DetailBandSurface extends BandSurface {
                protected _initMultiColumn(): void;
                _control: DetailBand;
            }
            module Metadata {
                var hierarchyPrintOptions: Analytics.Utils.ISerializationInfo;
                var detailBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesDetail: string[];
            }
            class DetailReportBand extends BandViewModel {
                static addBandToContainer(container: DetailReportBand | Controls.ReportViewModel, control: BandViewModel): void;
                dispose(): void;
                initHeight(): void;
                createChildsArray(band: any, serializer: Analytics.Utils.ModelSerializer): void;
                addChild(control: Analytics.Elements.IElementViewModel): void;
                constructor(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                dataMember: ko.Observable<string> | ko.Computed<string>;
                dataSource: ko.Observable<Data.ObjectStorageItem>;
                _filterString: ko.Observable<string> | ko.Computed<string>;
                filterString: Analytics.Widgets.FilterStringOptions;
                bands: ko.ObservableArray<BandViewModel>;
            }
            class DetailReportBandSurface extends BandSurface {
                dispose(): void;
                getChildrenCollection(): ko.ObservableArray<BandSurface>;
                createUnderCursor(): void;
                getTotalHeight(): number;
                getHeight(): number;
                getHasOwnRuler(): boolean;
                constructor(band: DetailReportBand, context: Analytics.Elements.ISurfaceContext);
                verticalBandsContainer: Internal.VerticalBandsContainerSurface;
                templateName: string;
                selectionTemplate: string;
                leftMarginTemplate: string;
            }
            module Metadata {
                var detailReportBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesDetailReport: string[];
            }
            interface ISortingSummary {
                enabled: boolean;
                Function: string;
                fieldName: string;
                ignoreNullValues: string;
                sortOrder: string;
                getPath: (propertyName: string) => string;
            }
            class GroupHeaderBand extends BandViewModel {
                dispose(): void;
                constructor(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                groupFields: ko.ObservableArray<GroupFieldModel>;
                sortingSummary: ISortingSummary;
            }
            module Metadata {
                var groupUnion: Analytics.Utils.ISerializationInfo;
                var groupFooterUnion: Analytics.Utils.ISerializationInfo;
                var sortingSummary: Analytics.Utils.ISerializationInfo;
                var groupHeaderBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var groupFooterBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesGroupFooter: string[];
                var popularPropertiesGroupHeader: string[];
            }
            class TopMarginBand extends BandViewModel {
                initHeight(): void;
            }
            class BottomMarginBand extends BandViewModel {
                initHeight(): void;
            }
            class BottomMarginSurface extends BandSurface {
                getBackgroundRect(): {
                    top: number;
                    bottom: any;
                    height: number;
                };
                parent: Controls.ReportSurface;
            }
            class PageFooterSurface extends BandSurface {
                getBackgroundRect(): {
                    top: number;
                    bottom: any;
                    height: number;
                };
                parent: Controls.ReportSurface;
            }
            module Metadata {
                var printOn: Analytics.Utils.ISerializationInfo;
                var pageBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesPageHeader: string[];
                var popularPropertiesPageFooter: string[];
            }
            class SubBandViewModel extends BandViewModel {
                constructor(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
            }
            class SubBandSurface extends BandSurface {
                getAbsolutePositionY(): number;
                getBackgroundRect(): {
                    top: number;
                    bottom: any;
                    height: number;
                };
                protected _initMultiColumn(): void;
                parent: BandSurface;
                leftMarginTemplate: string;
            }
            class VerticalBandViewModel extends BandViewModel {
                dispose(): void;
                initSize(): void;
                preInit(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer): void;
                constructor(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer);
                surface: VerticalBandSurface;
                controls: ko.ObservableArray<Controls.XRControlViewModel>;
                width: ko.Observable<number> | ko.Computed<number>;
                height: ko.Observable<number> | ko.Computed<number>;
                widthFromControls: ko.Computed<number>;
            }
            class VerticalBandSurface extends Analytics.Elements.SurfaceElementBase<VerticalBandViewModel> {
                static markerHeight: number;
                static _unitProperties: Analytics.Internal.IUnitProperties<VerticalBandViewModel>;
                isSomeParentCollapsed: ko.Observable<boolean>;
                private _resize;
                private _getRtlAbsolutePositionX;
                constructor(band: VerticalBandViewModel, context: Analytics.Elements.ISurfaceContext, unitProperties?: Analytics.Internal.IUnitProperties<VerticalBandViewModel>);
                getAbsolutePositionX(): number;
                updateAbsolutePosition(): void;
                minimumHeight(): number;
                minimumWidth(): number;
                collapsed: ko.Observable<boolean> | ko.Computed<boolean>;
                resizeHandles: ko.Computed<string>;
                templateName: string;
                selectiontemplate: string;
                contentSelectionTemplate: string;
                backgroundRect: ko.Computed<Analytics.Elements.IArea>;
                parent: Controls.ReportSurface | DetailReportBandSurface;
                readonly verticalBandsContainer: Internal.VerticalBandsContainerSurface;
                height: ko.Observable<number> | ko.Computed<number>;
                name: ko.Observable<string> | ko.Computed<string>;
                coordinateGridOptions: any;
                canResize: ko.Computed<boolean>;
                heightFromControls: ko.Computed<number>;
                widthFromControls: ko.Computed<number>;
            }
            module Metadata {
                var commonVerticalBandProperties: Analytics.Utils.ISerializationInfoArray;
                var bandLayout: Analytics.Utils.ISerializationInfo;
                var verticalHeaderBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesVerticalHeaderBand: string[];
                var verticalTotalBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesVerticalTotalBand: string[];
            }
            class VerticalDetailBandViewModel extends VerticalBandViewModel {
                dispose(): void;
                preInit(band: any, parent: Analytics.Elements.ElementViewModel, serializer?: Analytics.Utils.ModelSerializer): void;
                sortFields: ko.ObservableArray<GroupFieldModel>;
            }
            module Metadata {
                var verticalDetailBandSerializationInfo: Analytics.Utils.ISerializationInfoArray;
                var popularPropertiesVerticalDetailBand: string[];
            }
            module Internal {
                class VerticalBandsContainerSurface extends Analytics.Utils.Disposable {
                    private _parent;
                    markerWidth: ko.Observable<number>;
                    dispose(): void;
                    getBandPosition(): number;
                    isLocked(): boolean;
                    createScrollViewOptions(target: VerticalBandsContainerSurface, selection: Analytics.Internal.SurfaceSelection): {
                        direction: string;
                        showScrollbar: string;
                        useNative: boolean;
                        scrollByContent: boolean;
                        scrollByThumb: boolean;
                        onStart: () => void;
                        onScroll: (e: any) => void;
                        onEnd: () => void;
                    };
                    constructor(_parent: Controls.ReportSurface | BandSurface);
                    markerClick(selection: Analytics.Internal.SurfaceSelection): void;
                    getBandsWidth(bands: VerticalBandSurface[]): number;
                    _getTopOffset(): number;
                    name: string;
                    focused: ko.Computed<boolean>;
                    bandOffset: number;
                    leftOffset: ko.Computed<number>;
                    collapsed: ko.Computed<boolean>;
                    selected: ko.Computed<boolean>;
                    canResize: ko.Computed<boolean>;
                    width: ko.Observable<number> | ko.Computed<number>;
                    height: ko.Observable<number> | ko.Computed<number>;
                    _height: ko.Computed<number>;
                    leftMargin: ko.Computed<number>;
                    readonly visible: boolean;
                    templateName: string;
                    selectionTemplate: string;
                    vrulerTemplate: string;
                    leftMarginTemplate: string;
                    leftMarginSelectionTemplate: string;
                    verticalBands: ko.ObservableArray<VerticalBandSurface>;
                    minHeight: ko.Computed<number>;
                    bandPosition: ko.Computed<number>;
                    topOffset: ko.Computed<number>;
                    readonly zoom: ko.Observable<number> | ko.Computed<number>;
                    grayAreaWidth: ko.Observable<number> | ko.Computed<number>;
                    grayAreaLeft: ko.Observable<number> | ko.Computed<number>;
                    scrollOffset: ko.Observable<number>;
                    locked: ko.Computed<boolean>;
                }
            }
        }
        module Wizard {
            type WizardTypeString = "SingleQueryDataSourceWizard" | "DataSourceWizard" | "ReportWizard";
            type WizardRunType = "NewViaReportWizard" | "DataSourceWizard" | "DesignInReportWizard";
            type WizardType = Analytics.Wizard.DataSourceWizard | Analytics.Wizard.FullscreenDataSourceWizard | Wizard.FullscreenReportWizard;
            var LegacyReportWizardPageId: {
                ChooseDataMemberPage: string;
                SelectColumnsPage: string;
                AddGroupingLevelPage: string;
                ChooseSummaryOptionsPage: string;
                ChooseReportLayoutPage: string;
                ChooseReportStylePage: string;
            };
            var ReportWizardPageId: {
                SelectReportTypePage: string;
                ChooseAvailableDataSourcePage: string;
                SelectLabelTypePage: string;
                CustomizeLabelPage: string;
                SelectDataMembersPage: string;
                AddGroupingLevelPage: string;
                ChooseSummaryOptionsPage: string;
                ConfigureReportPageSettingsPage: string;
                ChooseReportColorSchemePage: string;
                SetReportTitlePage: string;
            };
            var FullscreenReportWizardPageId: {
                SelectReportTypePage: string;
                SelectDataSourcePage: string;
                SpecifySqlDataSourceSettingsPage: string;
                SpecifyJsonDataSourceSettingsPage: string;
                DefineReportLayoutPage: string;
                SpecifyPageSettingsPage: string;
                SpecifyLabelSettingsPage: string;
            };
            var FullscreenReportWizardSectionId: {
                ChooseAvailableDataSourcePage: string;
                SelectLabelTypePage: string;
                CustomizeLabelPage: string;
                SelectDataMembersPage_Members: string;
                SelectDataMembersPage_Fields: string;
                AddGroupFieldsPage: string;
                AddSummaryFieldsPage: string;
                ConfigurePageSettingsPage: string;
                SpecifyReportTitlePage: string;
                ChooseDataSourceTypePage: string;
                ChooseJsonSchemaPage: string;
                SpecifyJsonConnectionPage: string;
                ConfigureMasterDetailRelationshipsPage: string;
                ConfigureQueryParametersPage: string;
                ChooseSqlConnectionPage: string;
                ConfigureQueryPage: string;
                ChooseJsonSourcePage: string;
            };
        }
        module Internal {
            interface ICustomLabelInformation {
                Height: number;
                HorizontalPitch: number;
                LeftMargin: number;
                RightMargin: number;
                PaperKindDataId: number;
                TopMargin: number;
                BottomMargin: number;
                Unit: Wizard.GraphicsUnit;
                VerticalPitch: number;
                Width: number;
            }
            class CommonRequestModel {
                CustomLabelInformation: ICustomLabelInformation;
                IgnoreNullValuesForSummary: boolean;
                LabelProductId: number;
                LabelProductDetailId: number;
                ReportTitle: string;
                ReportType: number;
                constructor(state: Wizard.IReportWizardState);
            }
            class MasterDetailRequestModel extends CommonRequestModel {
                private _masterRelationMap;
                private _collectionByPath;
                DataSourceName: string;
                MasterDetailInfo: any;
                MasterDetailGroupsInfo: {
                    [key: string]: any;
                };
                MasterDetailSummariesInfo: {
                    [key: string]: {
                        ColumnName: string;
                        Flags: number;
                    }[];
                };
                Portrait: boolean;
                PaperKind: Utils.PaperKind;
                PaperSize: {
                    width: number;
                    height: number;
                };
                Margins: {
                    Top: number;
                    Right: number;
                    Bottom: number;
                    Left: number;
                };
                Unit: Wizard.GraphicsUnit;
                UseMasterDetailBuilder: boolean;
                constructor(state: Wizard.IReportWizardState);
            }
            class ReportWizardStateHelper {
                static applyDataBindings(state: Wizard.IReportWizardState, model: Controls.ReportViewModel): void;
                static applyPageSetup(state: Wizard.IReportWizardState, model: Controls.ReportViewModel): void;
            }
            class LegacyReportRequestModel extends CommonRequestModel {
                AdjustFieldWidth: boolean;
                Columns: Array<string>;
                ColumnInfo: {
                    Name: string;
                    DisplayName: string;
                    TypeSpecifics: number;
                }[];
                DataMemberName: {
                    "DisplayName": string;
                    "Name": string;
                    "DataMemberType": number;
                };
                DataSourceName: string;
                GroupingLevels: string[][];
                Layout: Wizard.ReportLayout;
                Portrait: boolean;
                ReportStyleId: Wizard.ReportStyle;
                SummaryOptions: {
                    ColumnName: string;
                    Flags: number;
                }[];
                UseMasterDetailBuilder: boolean;
                PaperKind: Utils.PaperKind;
                PaperSize: {
                    width: number;
                    height: number;
                };
                Margins: {
                    Top: number;
                    Right: number;
                    Bottom: number;
                    Left: number;
                };
                Unit: Wizard.GraphicsUnit;
                constructor(state: Wizard.ILegacyReportWizardState);
            }
        }
        module Wizard {
            enum ReportType {
                Empty = 3,
                Databound = 0,
                Vertical = 1,
                Label = 2
            }
            enum GraphicsUnit {
                World = 0,
                Display = 1,
                Pixel = 2,
                Point = 3,
                Inch = 4,
                Document = 5,
                Millimeter = 6
            }
            interface IPageSetup {
                paperKind: string;
                unit: GraphicsUnit;
                width: number;
                height: number;
                landscape?: boolean;
                marginLeft: number;
                marginRight: number;
                marginTop: number;
                marginBottom: number;
            }
            interface ILegacyReportWizardState extends IReportWizardState {
                dataMemberPath?: string;
                dataMemberInfo?: Analytics.Utils.IDataMemberInfo;
                fields?: Array<Analytics.Utils.IDataMemberInfo>;
                groups?: string[][];
                summaryOptionsColumns?: Array<Analytics.Utils.IDataMemberInfo>;
                summaryOptions?: Array<Internal.ISummaryOptions>;
                ignoreNullValuesForSummary?: boolean;
                dataSource?: string;
                newDataSource?: string;
                fitFieldsToPage?: boolean;
                layout?: Wizard.ReportLayout;
                portrait?: boolean;
                style?: Wizard.ReportStyle;
            }
            interface IColorSchemeState {
                baseColor?: string;
                name?: string;
            }
            interface IReportTitleState {
                reportTitle?: string;
            }
            interface IReportWizardState extends Analytics.Wizard.IDataSourceWizardState {
                pageSetup: IPageSetup;
                colorScheme: IColorSchemeState;
                reportType?: ReportType;
                labelDetails?: ILabelDetails;
                reportTitle?: string;
                dataMember?: string;
                masterDetailInfoCollection?: Internal.IMasterDetailQueryInfo[];
                masterDetailGroups?: any[];
                masterDetailSummaryOptionsColumns?: any;
                ignoreNullValuesForSummary?: boolean;
                dataSource?: string;
                newDataSource?: string;
                masterDetailSummariesInfo?: {
                    [key: string]: {
                        column: DevExpress.Analytics.Utils.IDataMemberInfo;
                        summaryFunctions: number[];
                    }[];
                };
            }
            var defaultPageSetupState: Wizard.IPageSetup;
            var defaultReportWizardState: IReportWizardState;
            function createReportWizardState(reportViewModel?: Controls.ReportViewModel): IReportWizardState;
            class ReportWizard extends DevExpress.Analytics.Wizard.PopupWizard {
                private _reportWizardOptions;
                protected _callBeforeFinishHandler(state: any, wizardModel?: any): void;
                protected _callAfterFinishHandler(state: any, result: any): void;
                constructor(pageFactory: Analytics.Wizard.PageFactory, _reportWizardOptions: _ReportWizardOptions);
                initialize(state?: IReportWizardState): void;
                start(finishCallback?: (state: IReportWizardState) => JQueryPromise<any>): void;
                _requestModelType: typeof Internal.MasterDetailRequestModel;
                title: any;
            }
            class ReportWizardPageIterator extends DevExpress.Analytics.Wizard.MultiQueryDataSourceWizardPageIterator<IReportWizardState> {
                private _reportWizardOptions;
                constructor(pagesFactory: Analytics.Wizard.PageFactory, stateManager: Analytics.Wizard.StateManager, _reportWizardOptions: _ReportWizardOptions);
                getNextPageId(pageId?: string): string;
            }
            function _registerCommonReportWizardPages(factory: Analytics.Wizard.PageFactory, reportWizardOptions: _ReportWizardOptions): void;
            function _registerReportWizardPages(factory: Analytics.Wizard.PageFactory, reportWizardOptions: _ReportWizardOptions): void;
            function _createReportWizard(reportWizardOptions: _ReportWizardOptions): ReportWizard;
            interface ISelectReportTypePageOptions {
                canCreateDatabound: boolean | (() => boolean);
                showVertical?: boolean;
            }
            class SelectReportTypePage extends DevExpress.Analytics.Wizard.ChooseDataSourceTypePage {
                private _options;
                constructor(_options: ISelectReportTypePageOptions);
                _addDataboundReportType(): void;
                canNext(): boolean;
                canFinish(): boolean;
                commit(): JQueryPromise<{}>;
                initialize(state: any): JQueryPromise<{}>;
                _extendCssClass: (rightPath: any) => string;
            }
            class ChooseDataSourceTypePage extends Analytics.Wizard.ChooseDataSourceTypePage {
                constructor(dataSourceWizardOptions: _ReportWizardOptions);
            }
            function _registerSelectReportTypePage(factory: Analytics.Wizard.FullscreenWizardPageFactory, options: ISelectReportTypePageOptions): void;
            function _registerChooseDataSourceTypePage(factory: Analytics.Wizard.PageFactory, dataSourceWizardOptions: _ReportWizardOptions): void;
            class CustomizeLabelPage extends DevExpress.Analytics.Wizard.WizardPageBase {
                static _CONVERSION_COEEFICIENT: number;
                private _id;
                private _labelWidth;
                private _labelHeight;
                private _horizontalPitch;
                private _verticalPitch;
                private _topMargin;
                private _leftMargin;
                private _rightMargin;
                private _bottomMargin;
                private _getFormattedValueInUnits;
                private _getLabelsCount;
                private _rowsCount;
                private _columnsCount;
                private _pageHeight;
                private _pageWidth;
                constructor();
                canNext(): boolean;
                canFinish(): boolean;
                initialize(labelDetails: Wizard.ILabelDetails): JQueryPromise<{
                    labelProducts: ILabelProduct[];
                    paperKinds: IPaperKind[];
                    labelDetails: ILabelDetails[];
                }>;
                commit(): JQueryPromise<ILabelDetails>;
                _labelData: {
                    labelProducts: Wizard.ILabelProduct[];
                    paperKinds: Wizard.IPaperKind[];
                    labelDetails: Wizard.ILabelDetails[];
                };
                paperKinds: () => IPaperKind[];
                _selectedPaperSize: ko.Observable<IPaperKind>;
                unit: ko.Observable<GraphicsUnit>;
                _stepUnit: ko.PureComputed<0.1 | 0.01>;
                labelWidth: ko.Computed<number>;
                labelHeight: ko.Computed<number>;
                horizontalPitch: ko.Computed<number>;
                verticalPitch: ko.Computed<number>;
                topMargin: ko.Computed<number>;
                leftMargin: ko.Computed<number>;
                rightMargin: ko.Computed<number>;
                bottomMargin: ko.Computed<number>;
                _labelsCountText: ko.PureComputed<string>;
                _pageSizeText: ko.PureComputed<string>;
                static _getPageSizeText(width: number, height: number, unit: GraphicsUnit): string;
                _units: {
                    text: any;
                    value: GraphicsUnit;
                }[];
            }
            function _registerCustomizeLabelPage(factory: DevExpress.Analytics.Wizard.PageFactory): void;
        }
        module Internal {
            var labelReportWizardPromise: JQueryPromise<{
                labelProducts: Wizard.ILabelProduct[];
                paperKinds: Wizard.IPaperKind[];
                labelDetails: Wizard.ILabelDetails[];
            }>;
            function initializeLabelReportWizardPromise(): void;
        }
        module Wizard {
            class SelectLabelTypePage extends DevExpress.Analytics.Wizard.WizardPageBase {
                constructor();
                initialize(state: IReportWizardState): JQueryPromise<{
                    labelProducts: ILabelProduct[];
                    paperKinds: IPaperKind[];
                    labelDetails: ILabelDetails[];
                }>;
                canFinish(): boolean;
                commit(): JQueryPromise<IReportWizardState>;
                _selectedPaperSize: ko.Computed<IPaperKind>;
                _labelData: {
                    labelProducts: Wizard.ILabelProduct[];
                    paperKinds: Wizard.IPaperKind[];
                    labelDetails: Wizard.ILabelDetails[];
                };
                _selectedLabelProduct: ko.Observable<ILabelProduct>;
                _selectedLabelDetails: ko.Observable<ILabelDetails>;
                _labelDetails: ko.Observable<any>;
                _width: ko.PureComputed<string>;
                _height: ko.PureComputed<string>;
                _paperType: ko.PureComputed<string>;
                _pageSizeText: ko.PureComputed<string>;
            }
            function _registerSelectLabelTypePage(factory: DevExpress.Analytics.Wizard.PageFactory): void;
            function _convertToStateDataSource(dataSource: any): string;
            function _restoreDataSourceFromState(dataSource: any): any;
            class ChooseAvailableDataSourcePage extends Analytics.Wizard.ChooseAvailableItemPage {
                commit(): JQueryPromise<{}>;
                _getSelectedItem(state: any): any;
                readonly createNewOperationText: any;
            }
            function _registerChooseAvailableDataSourcePage(factory: Analytics.Wizard.PageFactory, reportWizardOptions: _ReportWizardOptions): void;
            class SelectDataMembersPage extends Analytics.Wizard.WizardPageBase {
                private _fieldListCallBack;
                private _hideDataMemberSubItems;
                private _rootItems;
                private _dataMemberSelectedPath;
                private _fieldSelectedPath;
                private _dataMemberItemsProvider;
                private _fieldMemberItemsProvider;
                private _availableFieldsController;
                private _dataSource;
                private _checkedDataMembers;
                private _checkedFields;
                private initialFullDataMember;
                private _wrapFieldListCallback;
                private readonly dataSourcePath;
                private _showDataSource;
                private getDataMemberSelectedPath;
                private _beginInternal;
                private _afterCheckToggled;
                private _processFields;
                private _processNode;
                private _afterCheckToggledFields;
                private _createMasterDetailTreeNode;
                private _createMasterDetailFirstTabTreeNode;
                private _createMasterDetailLeafTreeNode;
                canNext(): boolean;
                canFinish(): boolean;
                constructor(_fieldListCallBack: Internal.IReportWizardFieldsCallback, _hideDataMemberSubItems?: boolean);
                selectDataMember(dataMemberPath: string): void;
                selectAllDataMembers(): void;
                selectDataField(dataFieldPath: string): void;
                selectDataFields(dataMemberPath: string): void;
                selectAllDataFields(): void;
                _dataMemberFieldListModel: Analytics.Widgets.Internal.ITreeListOptions;
                _fieldMemberFieldListModel: Analytics.Widgets.Internal.ITreeListOptions;
                initialize(state: IReportWizardState): JQueryPromise<any>;
                _haveCheckedFields(): boolean;
                commit(): JQueryPromise<{}>;
                _showFirstLevelDataMembers: ko.Observable<boolean>;
                _multiSelectMode: boolean;
                _selectDataMembersCaption: any;
                _selectDataFieldsCaption: any;
            }
            function _registerSelectDataMembersPage(factory: Analytics.Wizard.PageFactory, reportWizardOptions: _ReportWizardOptions, pageId?: string): void;
            function _fillTreeQueries(reportTree: any, queries: Internal.IMasterDetailQueryInfo[], level: number, parentDisplayName?: string): any;
            class AddGroupingLevelPage extends Analytics.Wizard.WizardPageBase {
                private _availableColumns;
                private _groupingLevels;
                private _setData;
                private _masterDetailGroups;
                constructor();
                canFinish(): boolean;
                _addNewGroup: () => void;
                _appendFieldsToGroup: () => void;
                _removeGroup: () => void;
                _isCreateGroupEnabled(): boolean;
                _isAppendToGroupEnabled(): boolean;
                _isRemoveGroupEnabled(): boolean;
                _moveUp: () => void;
                _moveDown: () => void;
                _isMoveUpEnabled(): boolean;
                _isMoveDownEnabled(): boolean;
                _fieldDblClick: (field: any) => void;
                _fieldClick: (e: {
                    itemData: any;
                }) => void;
                _groupDblClick: (group: any) => void;
                _groupClick: (e: {
                    itemData: any;
                }) => void;
                initialize(state: IReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<{}>;
                _currentPath: ko.Observable<string>;
                _currentFields: ko.Observable<Internal.ListViewModel<string>>;
                _currentGroups: ko.Observable<Internal.ListViewModel<{
                    fields: ko.ObservableArray<string>;
                }>>;
                _fieldCaption: any;
                _groupCaption: any;
                _reportTree: ko.ObservableArray<Internal.IMasterDetailReportTree>;
            }
            function _registerAddGroupingLevelPage(factory: Analytics.Wizard.PageFactory): void;
            interface IGroupFieldDataMemberInfo extends Analytics.Utils.IDataMemberInfo {
                visible?: ko.Observable<boolean>;
            }
            class _GroupsFieldStore extends Analytics.Utils.Disposable {
                private _onChange;
                dispose(): void;
                dataSource: ko.ObservableArray<IGroupFieldDataMemberInfo>;
                constructor(query: Internal.IMasterDetailReportTree, _onChange: () => void);
                getSelectedFieldsFlat(): IGroupFieldDataMemberInfo[];
                getSelectedFields(): string[][];
                groups: ko.ObservableArray<_GroupField>;
                isCreateGroupEnabled(): boolean;
                path: string;
                addGroupText: () => any;
                displayName: string;
                add(): void;
                remove(index: any): void;
                moveUpDisabled(index: any): boolean;
                moveDownDisabled(index: any): boolean;
                moveup(index: any): void;
                movedown(index: any): void;
            }
            class _GroupField extends Analytics.Utils.Disposable {
                private _store;
                private _onChange;
                private _updateDataSource;
                constructor(_store: _GroupsFieldStore, _onChange: () => void);
                getOptions(options: any): any;
                value: any;
                fields: ko.ObservableArray<string>;
            }
            class AddGroupFieldsPage extends Analytics.Wizard.WizardPageBase {
                dispose(): void;
                canFinish(): boolean;
                private _mergeGroups;
                initialize(state: IReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<{}>;
                _reportTree: Internal.IMasterDetailReportTree[];
                _groupInfos: ko.ObservableArray<Wizard._GroupsFieldStore>;
            }
            function _registerAddGroupFieldsPage(factory: Analytics.Wizard.PageFactory): void;
            class ChooseSummaryOptionsPage extends Analytics.Wizard.WizardPageBase {
                private _allColumns;
                private _masterDetailColumns;
                private _currentDataMember;
                private _createSummaryInfo;
                private _createNewItemIfNeed;
                private _changeQuery;
                constructor();
                _removeSummaryInfo(info: Internal.SummaryInfo): void;
                canFinish(): boolean;
                _toggleIgnoreNullValues: () => void;
                initialize(state: IReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<{}>;
                _summaryOptions: ko.ObservableArray<Internal.SummaryOptionsWrapper>;
                ignoreNullValues: ko.Observable<boolean>;
                _template: string;
                _reportTree: ko.ObservableArray<Internal.IMasterDetailReportTree>;
                _currentPath: ko.Observable<string>;
                _availableFields: ko.ObservableArray<any>;
                _displayedFields: {
                    [key: string]: ko.ObservableArray<any>;
                };
                _summaryInfos: ko.ObservableArray<Internal.SummaryInfo>;
                _summaryInfoMapByDataMember: {
                    [key: string]: Internal.SummaryInfo[];
                };
                _selectFieldToSummaryCaption: any;
                _fieldsCaption: any;
                _summaryFunctionCaption: any;
                _ignoreNullValuesCaption: any;
            }
            function _registerChooseSummaryOptionsPage(factory: Analytics.Wizard.PageFactory): void;
            class AddSummaryFieldsPage extends Analytics.Wizard.WizardPageBase {
                dispose(): void;
                private _fillTreeQueries;
                private _createSummaryInfo;
                private _createNewItemIfNeed;
                private _getParentName;
                private _flat;
                _removeSummaryInfo(info: Internal.SummaryInfoFieldlist): void;
                canFinish(): boolean;
                _toggleIgnoreNullValues: () => void;
                _updateSummaries(flatlist: Internal.ISummaryDataMemberInfo[]): void;
                initialize(state: IReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<{}>;
                _fieldListProvider: ko.Observable<Analytics.Utils.IItemsProvider>;
                ignoreNullValues: ko.Observable<boolean>;
                _template: string;
                _reportTree: ko.ObservableArray<Internal.ISummaryDataMemberInfo>;
                _availableFieldsCount: ko.Observable<number>;
                _summaryInfos: ko.ObservableArray<Internal.SummaryInfoFieldlist>;
                _selectFieldToSummaryCaption: any;
                _fieldsCaption: any;
                _summaryFunctionCaption: any;
                _ignoreNullValuesCaption: any;
            }
            function _registerAddSummaryFieldsPage(factory: Analytics.Wizard.PageFactory): void;
            interface IPageSettings {
                width: ko.Observable<number>;
                height: ko.Observable<number>;
                marginLeft: ko.Observable<number>;
                marginRight: ko.Observable<number>;
                marginTop: ko.Observable<number>;
                marginBottom: ko.Observable<number>;
            }
            class PreviewPageHelper extends Analytics.Utils.Disposable {
                private cachePagePreviewElement;
                constructor(settings?: IPageSettings);
                updatePageSettings(pageSetup: IPageSetup): void;
                width: ko.Observable<number>;
                height: ko.Observable<number>;
                marginLeft: ko.Observable<number>;
                marginRight: ko.Observable<number>;
                marginTop: ko.Observable<number>;
                marginBottom: ko.Observable<number>;
                previewPageWidth: ko.Computed<number>;
                previewPageHeight: ko.Computed<number>;
                previewTopMargin: ko.Computed<number>;
                previewRightMargin: ko.Computed<number>;
                previewBottomMargin: ko.Computed<number>;
                previewLeftMargin: ko.Computed<number>;
                pagePreviewElement: ko.Observable<JQuery>;
            }
            class ConfigureReportPageSettingsPage extends Analytics.Wizard.WizardPageBase {
                canFinish(): boolean;
                constructor();
                paperKind: ko.Observable<string>;
                landscape: ko.Observable<boolean>;
                unit: ko.Computed<GraphicsUnit>;
                width: ko.Observable<number>;
                height: ko.Observable<number>;
                fixedSize: ko.Computed<boolean>;
                marginLeft: ko.Observable<number>;
                marginRight: ko.Observable<number>;
                marginTop: ko.Observable<number>;
                marginBottom: ko.Observable<number>;
                valueFormat: ko.Computed<string>;
                lookupData: {
                    paperKind: {
                        value: string;
                        displayName: string;
                    }[];
                    unit: {
                        value: GraphicsUnit;
                        displayName: string;
                    }[];
                };
                private _unit;
                previewPageHelper: PreviewPageHelper;
                initialize(state: Wizard.IPageSetup): JQueryPromise<{}>;
                commit(): JQueryPromise<{}>;
            }
            function _applyPageSetting(data: Wizard.IPageSetup, state: Wizard.IPageSetup): void;
            function _registerConfigureReportPageSettingsPage(factory: Analytics.Wizard.PageFactory): void;
            class ChooseReportColorSchemePage extends Analytics.Wizard.WizardPageBase {
                constructor();
                addColorScheme(name: string, color: string, position?: number): void;
                removeColorScheme(...names: string[]): void;
                removeAllColorSchemes(): void;
                setCustomColor(color: string): void;
                _applyScheme(data: Wizard.ColorScheme): void;
                canFinish(): boolean;
                _scheme: ko.Observable<Wizard.ColorScheme>;
                _customColorScheme: CustomColorScheme;
                _lookupData: {
                    scheme: ColorScheme[];
                };
                initialize(state: IColorSchemeState): JQueryPromise<{}>;
                commit(): JQueryPromise<{}>;
            }
            function _applyColorSchemeState(data: IColorSchemeState, state: IColorSchemeState): void;
            function _registerChooseReportColorSchemePage(factory: Analytics.Wizard.PageFactory): void;
            class SetReportTitlePage extends Analytics.Wizard.WizardPageBase {
                initialize(data: IReportTitleState): JQueryPromise<{}>;
                canNext(): boolean;
                canFinish(): boolean;
                commit(): JQueryPromise<{}>;
                reportTitle: ko.Observable<string>;
            }
            function _registerSetReportTitlePage(factory: Analytics.Wizard.PageFactory): void;
            class ConfigurePageSettingsPage extends Analytics.Wizard.WizardPageBase {
                _configureReportPageSettingsPage: ConfigureReportPageSettingsPage;
                _colorSchemePage: ChooseReportColorSchemePage;
                _colorSchemePageVisible: boolean;
                dispose(): void;
                addColorScheme(name: string, color: string, position?: number): void;
                removeColorScheme(...names: string[]): void;
                removeAllColorSchemes(): void;
                setCustomColor(color: string): void;
                onChange(callback: any): void;
                canNext(): boolean;
                canFinish(): boolean;
                initialize(state: IReportWizardState): JQueryPromise<any>;
                commit(): JQueryPromise<{}>;
            }
            function _registerConfigureReportPageSettingsSection(factory: Analytics.Wizard.PageFactory): void;
        }
        module Internal {
            function overrideJsonDataSourceWizardPage(factory: Analytics.Wizard.PageFactory, pageId: string, meta: Analytics.Wizard.IWizardPageMetadata<Analytics.Wizard.IWizardPage>): void;
            function overrideSqlDataSourceWizardPage(factory: Analytics.Wizard.PageFactory, pageId: string, meta: Analytics.Wizard.IWizardPageMetadata<Analytics.Wizard.IWizardPage>): void;
            class DataSourceWizardHelper {
                private _page;
                private _callback;
                constructor(_page: Analytics.Wizard.IWizardPage, _callback: (dataSource: any) => JQueryPromise<Designer.Internal.IDataSourceInfo>);
                commit(superCommit: () => JQueryPromise<any>, createDataSource: (state: any) => any): JQueryPromise<any>;
            }
        }
        module Wizard {
            class ChooseJsonSchemaPage extends Analytics.Wizard.ChooseJsonSchemaPage {
                private _dataSourceWizardHelper;
                private _dataSourceId;
                constructor(createJsonDataSourceInfo: (dataSource: Analytics.Data.JsonDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>);
                initialize(state: any): JQueryPromise<{}>;
                commit(): JQueryPromise<any>;
            }
            function _registerChooseJsonSchemaPage(factory: Analytics.Wizard.PageFactory, callbacks: Internal.IReportWizardCallbacks): void;
            class ConfigureMasterDetailRelationshipsPage extends Analytics.Wizard.ConfigureMasterDetailRelationshipsPage {
                private _dataSourceWizardHelper;
                constructor(createSqlDataSourceInfo: (dataSource: Analytics.Data.SqlDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>, sqlDataSourceResultSchema: any);
                initialize(state: any): JQueryPromise<Analytics.Data.ResultSet>;
                commit(): JQueryPromise<any>;
            }
            function _registerConfigureMasterDetailRelationshipsPage(factory: Analytics.Wizard.PageFactory, callbacks: Internal.IReportWizardCallbacks): void;
            class MultiQueryConfigurePage extends Analytics.Wizard.MultiQueryConfigurePage {
                private _dataSourceWizardHelper;
                constructor(reportWizardOptions: _ReportWizardOptions);
                _getQueriesCount(): any;
                _canEditQueryParameters(): any;
                initialize(state: any): JQueryPromise<Analytics.Data.Utils.ISqlQueryViewModel>;
                commit(): JQueryPromise<any>;
            }
            function _registerMultiQueryConfigurePage(factory: Analytics.Wizard.PageFactory, reportWizardOptions: _ReportWizardOptions): void;
            class MultiQueryConfigureParametersPage extends Analytics.Wizard.MultiQueryConfigureParametersPage {
                private createSqlDataSourceInfo;
                private _dataSourceWizardHelper;
                constructor(createSqlDataSourceInfo: (dataSource: Analytics.Data.SqlDataSource) => JQueryPromise<Designer.Internal.IDataSourceInfo>, parametersConverters?: any, requestWrapper?: any);
                initialize(state: any): JQueryPromise<{}>;
                commit(): JQueryPromise<any>;
            }
            function _registerMultiQueryConfigureParametersPage(factory: Analytics.Wizard.PageFactory, callbacks: Internal.IReportWizardCallbacks): void;
            class LegacyAddGroupingLevelPage extends DevExpress.Analytics.Wizard.WizardPageBase {
                private initialFields;
                fields: Internal.ListViewModel<string>;
                groups: Internal.ListViewModel<{
                    fields: ko.ObservableArray<string>;
                }>;
                canFinish(): boolean;
                addNewGroup: () => void;
                appendFieldsToGroup: () => void;
                removeGroup: () => void;
                isCreateGroupEnabled(): boolean;
                isAppendToGroupEnabled(): boolean;
                isRemoveGroupEnabled(): boolean;
                moveUp: () => void;
                moveDown: () => void;
                isMoveUpEnabled(): boolean;
                isMoveDownEnabled(): boolean;
                fieldDblClick: (field: any) => void;
                fieldClick: (e: {
                    itemData: any;
                }) => void;
                groupDblClick: (group: any) => void;
                groupClick: (e: {
                    itemData: any;
                }) => void;
                initialize(state: ILegacyReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ILegacyReportWizardState>;
            }
            function _registerLegacyAddGroupingLevelPage(factory: DevExpress.Analytics.Wizard.PageFactory): void;
            class LegacyChooseReportLayoutPage extends DevExpress.Analytics.Wizard.WizardPageBase {
                private _isGroupedReport;
                private _reportLayoutTypes;
                private _groupedReportLayoutsTypes;
                canFinish(): boolean;
                initialize(state: ILegacyReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ILegacyReportWizardState>;
                toggleFitFieldsToPage: () => void;
                selectedLayoutType: ko.Observable<LayoutTypeItem>;
                fitFieldsToPage: ko.Observable<boolean>;
                pageOrientationItems: PageOrientationItem[];
                selectedPageOrientation: ko.Observable<PageOrientationItem>;
                layoutTypeItems: ko.PureComputed<LayoutTypeItem[]>;
                layoutTypeItemClick: (item: LayoutTypeItem) => void;
                isSelected: (item: LayoutTypeItem) => boolean;
            }
            function _registerLegacyChooseReportLayoutPage(factory: DevExpress.Analytics.Wizard.PageFactory): void;
            class LegacyChooseReportStylePage extends DevExpress.Analytics.Wizard.WizardPageBase {
                canFinish(): boolean;
                initialize(state: ILegacyReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ILegacyReportWizardState>;
                reportStyleItems: ReportStyleItem[];
                selectedReportStyle: ko.Observable<ReportStyleItem>;
            }
            function _registerLegacyChooseReportStylePage(factory: DevExpress.Analytics.Wizard.PageFactory): void;
            class LegacyChooseSummaryOptionsPage extends DevExpress.Analytics.Wizard.WizardPageBase {
                private _columns;
                canFinish(): boolean;
                initialize(state: ILegacyReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ILegacyReportWizardState>;
                summaryOptions: ko.ObservableArray<Internal.SummaryOptionsWrapper>;
                ignoreNullValues: ko.Observable<boolean>;
                toggleIgnoreNullValues: () => void;
            }
            function _registerLegacyChooseSummaryOptionsPage(factory: DevExpress.Analytics.Wizard.PageFactory): void;
            class LegacySelectColumnsPage extends DevExpress.Analytics.Wizard.WizardPageBase {
                private _fieldListsCallback;
                private _selectedPath;
                private _fields;
                constructor(getFieldListItems: Internal.IReportWizardFieldsCallback);
                canFinish(): boolean;
                canNext(): boolean;
                selectedPath(): any;
                reset(): void;
                initialize(state: ILegacyReportWizardState): JQueryPromise<{}>;
                commit(): JQueryPromise<ILegacyReportWizardState>;
                isSelectEnable(): boolean;
                isUnselectEnable(): boolean;
                select: () => void;
                selectAll: () => void;
                unselect: () => void;
                unselectAll: () => void;
                availableFieldDblClick: (field: any) => void;
                availableFieldClick: (e: {
                    itemData: any;
                }) => void;
                selectedFieldDblClick: (field: any) => void;
                selectedFieldClick: (e: {
                    itemData: any;
                }) => void;
                availableFields: Internal.ListViewModel<Analytics.Utils.IDataMemberInfo>;
                selectedFields: Internal.ListViewModel<Analytics.Utils.IDataMemberInfo>;
            }
            function _registerLegacySelectColumnsPage(factory: DevExpress.Analytics.Wizard.PageFactory, fieldListItemsCallback: Internal.IReportWizardFieldsCallback): void;
            class LegacyChooseDataMemberPage extends DevExpress.Analytics.Wizard.WizardPageBase {
                private _rootItems;
                private _selectedPath;
                private _fieldListCallBack;
                private _createSqlDataSourceInfo;
                private _dataSource;
                private _hideDataMemberSubItems;
                private _wrapFieldListCallback;
                private readonly dataSourcePath;
                private _beginInternal;
                constructor(reportWizardOptions: _ReportWizardOptions);
                canNext(): boolean;
                canFinish(): boolean;
                initialize(state: IReportWizardState): JQueryPromise<any>;
                commit(): JQueryPromise<ILegacyReportWizardState>;
                scrollViewHeight: string;
                fieldListModel: {
                    itemsProvider: Analytics.Internal.FieldListProvider;
                    selectedPath: any;
                    treeListController: Analytics.Widgets.Internal.DataMemberTreeListController;
                };
            }
            function _registerLegacyChooseDataMemberPage(factory: DevExpress.Analytics.Wizard.PageFactory, reportWizardOptions: _ReportWizardOptions): void;
            class LegacyReportWizard extends DevExpress.Analytics.Wizard.PopupWizard {
                private _reportWizardOptions;
                protected _callBeforeFinishHandler(state: any, wizardModel?: any): void;
                protected _callAfterFinishHandler(state: any, result: any): void;
                constructor(pageFactory: any, _reportWizardOptions: _ReportWizardOptions);
                initialize(state?: IReportWizardState): void;
                start(finishCallback?: (state: IReportWizardState) => JQueryPromise<any>): void;
                _requestModelType: typeof Internal.LegacyReportRequestModel;
                title: any;
            }
            class LegacyReportWizardPageIterator extends Analytics.Wizard.MultiQueryDataSourceWizardPageIterator<ILegacyReportWizardState> {
                constructor(pageFactory: Analytics.Wizard.PageFactory, stateManager: Analytics.Wizard.StateManager, reportWizardOptions: _ReportWizardOptions);
                getNextPageId(pageId: string): string;
            }
            function _createLegacyReportWizard(reportWizardOptions: _ReportWizardOptions): LegacyReportWizard;
            class SelectDataSourcePage extends Analytics.Wizard.FullscreenWizardPage {
                private reportWizardOptions;
                constructor(reportWizardOptions: _ReportWizardOptions);
                registerSections(): void;
                getNextSectionId(sectionId: any): string;
            }
            function _registerSelectDataSourcePage(factory: Analytics.Wizard.FullscreenWizardPageFactory, reportWizardOptions: _ReportWizardOptions): void;
            class SpecifySqlDataSourceSettingsPage extends Analytics.Wizard.SpecifySqlDataSourceSettingsPage {
                registerSections(): void;
                commit(): JQueryPromise<{}>;
            }
            function _registerSpecifySqlDataSourceSettingsPage(factory: Analytics.Wizard.FullscreenWizardPageFactory, wizardOptions: _ReportWizardOptions): void;
            class SpecifyJsonDataSourceSettingsPage extends Analytics.Wizard.SpecifyJsonDataSourceSettingsPage {
                registerSections(): void;
                canNext(): boolean;
            }
            function _registerSpecifyJsonDataSourceSettingsPage(factory: Analytics.Wizard.FullscreenWizardPageFactory, wizardOptions: _ReportWizardOptions): void;
            class DefineReportLayoutPage extends Analytics.Wizard.FullscreenWizardPage {
                private _reportWizardOptions;
                constructor(_reportWizardOptions: _ReportWizardOptions);
                registerSections(): void;
                _beforeStart(): void;
                getNextSectionId(sectionId: string): string;
            }
            function _registerDefineReportLayoutPage(factory: Analytics.Wizard.FullscreenWizardPageFactory, reportWizardOptions: _ReportWizardOptions): void;
            class SpecifyPageSettingsPage extends Analytics.Wizard.FullscreenWizardPage {
                private _reportWizardOptions;
                constructor(_reportWizardOptions: _ReportWizardOptions);
                canNext(): boolean;
                canFinish(): boolean;
                registerSections(): void;
                getNextSectionId(sectionId: any): string;
            }
            function _registerSpecifyPageSettingsPage(factory: Analytics.Wizard.FullscreenWizardPageFactory, reportWizardOptions: _ReportWizardOptions): void;
            class SpecifyReportTitlePage extends Analytics.Wizard.WizardPageBase {
                constructor();
                private _getBrightness;
                private _fillTables;
                initialize(state: any): JQueryPromise<{}>;
                commit(): JQueryPromise<{}>;
                _reportTitlePlaceholder(): any;
                _foreColor: ko.Observable<string>;
                _masterDetailInfo: ko.ObservableArray<any>;
                reportTitle: ko.Observable<string> | ko.Computed<string>;
                _reportTitleVisible: boolean;
                _color: ko.Observable<string>;
                _previewPageHelper: PreviewPageHelper;
            }
            function _registerSpecifyReportTitlePage(factory: Analytics.Wizard.PageFactory): void;
            class SpecifyLabelSettingsPage extends Analytics.Wizard.FullscreenWizardPage {
                private _reportWizardOptions;
                constructor(_reportWizardOptions: _ReportWizardOptions);
                registerSections(): void;
                canNext(): boolean;
                canFinish(): boolean;
                getNextSectionId(sectionId: string): string;
            }
            function _registerSpecifyLabelSettingsPage(factory: Analytics.Wizard.FullscreenWizardPageFactory, reportWizardOptions: _ReportWizardOptions): void;
            class FullscreenReportWizard extends Analytics.Wizard.FullscreenWizard {
                private _reportWizardOptions;
                protected _callBeforeFinishHandler(state: any, wizardModel?: any): void;
                protected _callAfterFinishHandler(state: any, result: any): void;
                constructor(pageFactory: Analytics.Wizard.FullscreenWizardPageFactory, _reportWizardOptions: _ReportWizardOptions);
                _description(): any;
                initialize(state?: IReportWizardState): void;
                _requestModelType: typeof Internal.MasterDetailRequestModel;
                _availableDataSources: ko.Observable<Analytics.Internal.IDataSourceInfo[]> | ko.Computed<Analytics.Internal.IDataSourceInfo[]>;
            }
            class FullscreenReportWizardPageIterator extends DevExpress.Analytics.Wizard.PageIterator<IReportWizardState> {
                getNextPageId(pageId?: string): string;
            }
            function _registerFullscreenReportWizardPages(factory: Analytics.Wizard.PageFactory, reportWizardOptions: _ReportWizardOptions): void;
            function _createFullscreenReportWizard(reportWizardOptions: _ReportWizardOptions): FullscreenReportWizard;
        }
        module Widgets {
            var groups: DevExpress.Analytics.Internal.GroupObject;
        }
        module Tools {
            interface INavigateTab extends Analytics.Utils.IDisposable {
                displayName: ko.Computed<string>;
                isDirty: ko.Observable<boolean> | ko.Computed<boolean>;
                close?: JQueryDeferred<any>;
                icon?: string;
                context: ko.Observable<ReportDesignerContext> | ko.Computed<ReportDesignerContext>;
                undoEngine: Analytics.Utils.UndoEngine;
                report: ko.Computed<Controls.ReportViewModel>;
                url: ko.Computed<string>;
            }
            interface INavigateTabCallbacks {
                createContext: (report: Controls.ReportViewModel, url: string | ko.Observable<string> | ko.Computed<string>) => ReportDesignerContext;
                afterInititalize: (tab: NavigateTab) => void;
            }
            interface INavigateTabOptions {
                report: Controls.ReportViewModel;
                url: string | ko.Observable<string> | ko.Computed<string>;
                isReportLoading: ko.Observable<boolean> | ko.Computed<boolean>;
                callbacks: INavigateTabCallbacks;
            }
            class NavigateTab extends Analytics.Utils.Disposable implements INavigateTab {
                dispose(): void;
                private _generateDisplayName;
                private _createReport;
                private _createReportUrl;
                changeContext(report: Controls.ReportViewModel, reportUrl: string): void;
                resetIsModified(): void;
                refresh(): void;
                refreshSubreports(): void;
                constructor(options: INavigateTabOptions);
                private _callbacks;
                displayName: ko.Computed<string>;
                isDirty: ko.Observable<boolean> | ko.Computed<boolean>;
                isModified: ko.Observable<boolean> | ko.Computed<boolean>;
                close: JQueryDeferred<any>;
                icon: string;
                undoEngine: Analytics.Utils.UndoEngine;
                _isReportLoading: ko.Observable<boolean> | ko.Computed<boolean>;
                report: ko.Computed<Controls.ReportViewModel>;
                url: ko.Computed<string>;
                context: ko.Observable<ReportDesignerContext> | ko.Computed<ReportDesignerContext>;
            }
            interface INavigationOptions {
                report?: Controls.ReportViewModel;
                reportUrl?: ko.Observable<string> | ko.Computed<string>;
                callbacks?: Utils.IReportDesignerCustomizationHandler;
                allowMDI?: boolean;
                selection: Analytics.Internal.SurfaceSelection;
                initOptions: IDesignerContextOptionsInitOptions;
                knownEnums?: any;
            }
            class NavigateByReports extends Analytics.Utils.Disposable {
                private _designerReportModel;
                private _isReportLoading;
                private _removeTab;
                dispose(): void;
                private _closeTab;
                private _closeAll;
                private _getTabByControl;
                private _addTab;
                changeContext(report: Controls.ReportViewModel, reportUrl?: ko.Observable<string> | ko.Computed<string>): void;
                constructor(options: INavigationOptions);
                init(isLoading: ko.Observable<boolean>): void;
                removeTab(tab: any, force?: boolean): JQueryPromise<{}>;
                closeAll(): JQueryPromise<{}>;
                save: (tab: NavigateTab) => any;
                switch(tab: NavigateTab): void;
                goToSubreport(subreportSurface: Controls.XRSubreportSurface): void;
                addTab(report: Controls.ReportViewModel, url?: ko.Observable<string> | ko.Computed<string>): void;
                checkHeight(): void;
                currentTab: ko.Observable<NavigateTab> | ko.Computed<NavigateTab>;
                height: ko.Observable<number> | ko.Computed<number>;
                tabs: ko.ObservableArray<NavigateTab>;
                allowMDI: boolean;
                knownEnums: any;
                _callbacks: Utils.IReportDesignerCustomizationHandler;
                _selection: Analytics.Internal.SurfaceSelection;
                _initializeOptions: IDesignerContextOptionsInitOptions;
                _selectedIndex: ko.Observable<number> | ko.Computed<number>;
                selectedIndex: ko.Computed<number>;
            }
        }
        module Internal {
            class ReportMenuSettings extends DevExpress.Analytics.Internal.MenuSettings {
                appMenuVisible: ko.Observable<boolean>;
                dispose(): void;
                _$menuElement: JQuery;
                setMenuElement($element: any): void;
                toggleAppMenu: any;
                constructor();
                _toggleAppMenu(): void;
                generate(): {};
                isMenuCollapsed: ko.Observable<boolean>;
            }
        }
        interface IReportDesignerRootContext extends Analytics.Internal.IDesignerModel {
            fullScreen: ko.Computed<boolean>;
            _wizardRunner: Internal.WizardRunner;
            model: ko.Observable<Controls.ReportViewModel>;
            surface: ko.Observable<Controls.ReportSurface>;
            navigateByReports: Tools.NavigateByReports;
            reportUrls: ko.ObservableArray<IKeyValuePair<string>>;
            fieldListItemsExtenders: ko.Observable<Analytics.Internal.IItemsExtender[]>;
            validationMode: ko.Computed<boolean>;
            rootStyle: string;
            toolboxDragHandler: Internal.ReportToolboxDragDropHandler;
            isDirty: ko.Computed<boolean>;
            calculatedFieldsSource: ko.Computed<Internal.CalculatedFieldsSource>;
            parameters: ko.Computed<Internal.ParametersViewModel>;
            reportPreviewModel: any;
            fieldListActionProviders: Analytics.Internal.IActionsProvider[];
            wizard: Wizard.ReportWizard;
            dataSourceWizard: Analytics.Wizard.DataSourceWizard;
            multiQueryDataSourceWizard: Analytics.Wizard.MultiQueryDataSourceWizard;
            addOns: ko.ObservableArray<Analytics.Internal.IDesignerPart>;
            scriptsEditor: Internal.ScriptsEditor;
            state: any;
            events: ko.Computed<any[]>;
            gotoEvent: (functionName: any, eventName: any, model: any) => void;
            saveReportDialog: Tools.SaveAsReportDialog;
            saveReportDialogLight: Tools.SaveReportDialog;
            connections: Analytics.Wizard.IDataSourceWizardConnectionStrings;
            availableDataSources: Analytics.Internal.IDataSourceInfo[];
            openReportDialog: Tools.OpenReportDialog;
            styles: ko.Computed<ko.ObservableArray<Controls.StyleModel>>;
            formattingRuleSheet: ko.Computed<ko.ObservableArray<Controls.FormattingRule>>;
            reportExplorerProvider: Analytics.Internal.ObjectExplorerProvider;
            designMode: ko.Observable<boolean> | ko.Computed<boolean>;
            displayNameProvider: ko.Computed<Internal.DisplayNameProvider>;
            getDisplayNameByPath: (path: string, value: string) => JQueryPromise<string>;
            fieldListProvider: ko.Computed<Analytics.Internal.FieldListProvider>;
            dataBindingsProvider: ko.Computed<Analytics.Internal.FieldListProvider>;
            fieldListDataSources: ko.ObservableArray<Analytics.Internal.IDataSourceInfo>;
            reportItemsProvider: ko.Computed<Internal.ReportItemsProvider>;
            expressionDisplayNameProvider: ko.Computed<Internal.DisplayNameProvider>;
            dataSourceHelper: ko.Computed<Internal.DataSourceHelper>;
            selectedPath: ko.Observable<string> | ko.Computed<string>;
            controls: ko.Computed<Analytics.Internal.INamedValue[]>;
            bands: ko.Computed<Analytics.Internal.INamedValue[]>;
            isMenuCollapsed: ko.Observable<boolean>;
            chartDataSources: ko.Computed<Array<{
                displayName: string;
                value: any;
            }>>;
            getControls: (target: any) => ko.Computed<ko.Computed<Analytics.Internal.INamedValue[]>>;
            actionStorage: any;
            fieldDragHandler: Internal.FieldListDragDropHandler;
            runChartDesigner: (chart: Controls.XRChartSurface) => void;
            zoomStep: ko.Observable<number> | ko.Computed<number>;
            onViewPortScroll: (viewPort: HTMLElement) => void;
            updateSurfaceSize: () => void;
            openReport: (url: string) => void;
            getTabs: () => Tools.INavigateTab[];
            closeTab: (tab: Tools.INavigateTab, force?: boolean) => void;
        }
        interface IDesignerContextOptionsInitOptions {
            availableDataSources: Analytics.Internal.IDataSourceInfo[];
            state?: any;
        }
        interface IDesignerContextOptions {
            initializeOptions: IDesignerContextOptionsInitOptions;
            selection: Analytics.Internal.SurfaceSelection;
            report?: Controls.ReportViewModel;
            knownEnums?: any;
            url?: string | ko.Observable<string> | ko.Computed<string>;
            data?: any;
            dataSourceRefs?: any;
            designerCallbacks: Utils.IReportDesignerCustomizationHandler;
        }
        interface IReportDesignerContext {
            report: Controls.ReportViewModel;
            url: ko.Observable<string> | ko.Computed<string>;
            surface: Controls.ReportSurface;
            dataSourceHelper: Internal.DataSourceHelper;
            parameters: Internal.ParametersViewModel;
            fieldListDataSourceHelper: Internal.FieldListDataSourcesHelper;
            calcFieldsSource: Internal.CalculatedFieldsSource;
            fieldListItemsExtenders: Analytics.Internal.IItemsExtender[];
            fieldListProvider: Analytics.Internal.FieldListProvider;
            reportItemsProvider: Internal.ReportItemsProvider;
            dataBindingsProvider: Analytics.Internal.FieldListProvider;
            chartValueBindingProvider: Analytics.Internal.FieldListProvider;
            displayNameProvider: Internal.DisplayNameProvider;
            expressionDisplayNameProvider: Internal.DisplayNameProvider;
            controlsHelper: Internal.DesignControlsHelper;
            state: () => any;
        }
        class ReportDesignerContext extends Analytics.Utils.Disposable implements IReportDesignerContext {
            state: () => any;
            url: ko.Observable<string> | ko.Computed<string>;
            report: Controls.ReportViewModel;
            surface: Controls.ReportSurface;
            dataSourceHelper: Internal.DataSourceHelper;
            parameters: Internal.ParametersViewModel;
            fieldListDataSourceHelper: Internal.FieldListDataSourcesHelper;
            calcFieldsSource: Internal.CalculatedFieldsSource;
            fieldListItemsExtenders: Analytics.Internal.IItemsExtender[];
            fieldListProvider: Analytics.Internal.FieldListProvider;
            reportItemsProvider: Internal.ReportItemsProvider;
            dataBindingsProvider: Analytics.Internal.FieldListProvider;
            chartValueBindingProvider: Analytics.Internal.FieldListProvider;
            displayNameProvider: Internal.DisplayNameProvider;
            expressionDisplayNameProvider: Internal.DisplayNameProvider;
            controlsHelper: Internal.DesignControlsHelper;
            private _getChartAvailableSources;
            getInfo(): {
                propertyName: string;
                modelName: string;
            }[];
            isModelReady(): boolean;
            dispose(): void;
            constructor(options: IDesignerContextOptions);
        }
        module Internal {
            var QBRequestWrapper: QueryBuilder.Utils.RequestWrapper;
            interface IReportDesignerGeneratorSettings {
                selection?: Analytics.Internal.SurfaceSelection;
                rtl?: boolean;
                callbacks: {
                    designer?: Utils.IReportDesignerCustomizationHandler;
                    preview?: DevExpress.Reporting.Viewer.Utils.IPreviewCustomizationHandler;
                };
                reportStorageWebIsRegister?: boolean;
                allowMDI?: boolean;
                knownEnums?: DevExpress.Reporting.IEnumType[];
                reportUrl?: ko.Observable<string> | ko.Computed<string>;
                availableDataSources?: Analytics.Internal.IDataSourceInfo[];
                convertBindingsToExpressions?: string;
                state?: any;
            }
            interface IReportUriSettings {
                reportDesignerUri?: string;
                previewUri?: string;
            }
            interface PreviewOptions {
                element: Element;
                callbacks: DevExpress.Reporting.Viewer.Utils.IPreviewCustomizationHandler;
                localizationSettings?: Analytics.Internal.ILocalizationSettings;
                parametersInfo?: DevExpress.Reporting.Viewer.Parameters.IReportParametersInfo;
                handlerUri?: string;
                rtl?: boolean;
            }
            class WizardsInitializerSettings {
                private callbacks;
                private _doFinishCallback;
                registerReportWizardPages: (pageFactory: Analytics.Wizard.PageFactory) => void;
                registerMultiQueryDataSourceWizardPages: (pageFactory: Analytics.Wizard.PageFactory) => void;
                sqlDataSourceEditor: SqlDataSourceEditor;
                jsonDataSourceEditor: JsonDataSourceEditor;
                dataSourceActionProvider: DataSourceActions;
                dataSourceWizard: Analytics.Wizard.DataSourceWizard;
                multiQueryDataSourceWizard: Analytics.Wizard.MultiQueryDataSourceWizard | Analytics.Wizard.FullscreenDataSourceWizard;
                multipleQueriesWizardCallbacks: Analytics.Wizard.Internal.IMultiQueryDataSourceWizardCallbacks;
                reportWizard: Wizard.ReportWizard | Wizard.LegacyReportWizard | Wizard.FullscreenReportWizard;
                createSqlDataSourceWizard(disableCustomSql: any, itemsProvider?: any): Analytics.Wizard.DataSourceWizard;
                createSqlDataSourceEditor(settings: {
                    dataSourceHelper: ko.Observable<DataSourceHelper> | ko.Computed<DataSourceHelper>;
                    dataSourceWizard: Analytics.Wizard.DataSourceWizard;
                    model: ko.Observable<Controls.ReportViewModel> | ko.Computed<Controls.ReportViewModel>;
                    undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>;
                    fieldListProvider: ko.Observable<Analytics.Internal.FieldListProvider> | ko.Computed<Analytics.Internal.FieldListProvider>;
                }): void;
                createMultipleQueriesWizardCallbacks(itemsProvider?: any): void;
                createMultiQueryDataSourceWizard(disableCustomSql: any, multipleQueriesWizardCallbacks?: Analytics.Wizard.Internal.IMultiQueryDataSourceWizardCallbacks, canCreateJsonDataSource?: boolean): void;
                createReportWizard(settings: {
                    dataSourceHelper: ko.Observable<DataSourceHelper> | ko.Computed<DataSourceHelper>;
                    navigation: Tools.NavigateByReports;
                    isLoading: ko.Observable<boolean> | ko.Computed<boolean>;
                    isDirty: ko.Observable<boolean> | ko.Computed<boolean>;
                    state: () => any;
                    model: ko.Observable<Controls.ReportViewModel> | ko.Computed<Controls.ReportViewModel>;
                    undoEngine: ko.Observable<Analytics.Utils.UndoEngine> | ko.Computed<Analytics.Utils.UndoEngine>;
                    fieldListProvider: ko.Observable<Analytics.Internal.FieldListProvider> | ko.Computed<Analytics.Internal.FieldListProvider>;
                    data: Utils.IReportDesignerInitializationData;
                }): void;
                constructor(connectionStrings: Analytics.Wizard.IDataSourceWizardConnectionStrings, wizardSettings: Utils.IReportWizardSettings, callbacks: Utils.IReportDesignerCustomizationHandler, rtl: boolean);
                private reportWizardOptions;
                private multiQueryWizardOptions;
                private dataSourceWizardOptions;
            }
            class ReportDialogSettings {
                private _designerCallbacks;
                saveReportDialog: Tools.SaveAsReportDialog;
                saveReportDialogLight: Tools.SaveReportDialog;
                openReportDialog: Tools.OpenReportDialog;
                constructor(_designerCallbacks: Utils.IReportDesignerCustomizationHandler);
                createSaveReportDialog(reportUrls: ko.ObservableArray<IKeyValuePair<string>>): void;
                createSaveReportDialogLight(saveReportDialog?: Tools.SaveAsReportDialog): void;
                createOpenReportDialog(reportUrls: any, navigation: Tools.NavigateByReports): void;
            }
            class ReportDesignerInitializer extends Analytics.Internal.CommonDesignerGenerator<IReportDesignerRootContext> {
                private options;
                private _navigation;
                private _selection;
                private _sqlDataSourceEditor;
                private _jsonDataSourceEditor;
                private _dataSourceActionProvider;
                private _previewUri;
                private _dataBiningMode;
                private _parameters;
                private _calculatedFieldsSource;
                private _convertBindingsToExpressions;
                private _reportcontext;
                readonly reportContext: ko.Computed<IReportDesignerContext>;
                private _allowMDI;
                private _callbacks;
                private _customMergeEngine;
                readonly buildingModel: IReportDesignerRootContext;
                private readonly _designerCallbacks;
                subscribeIncomeReport(report: ko.Observable | ko.Computed, reportUrl?: ko.Observable<string> | ko.Computed<string>, dataSourceRefs?: any[]): this;
                private _addDisposable;
                private _getSubreportUrls;
                private _tryAddScriptEditor;
                private _getControls;
                addReportDialogs(func: (settings: ReportDialogSettings) => void): this;
                addFlagsAndInitProperties(element?: Element): this;
                addPreview(options: PreviewOptions): this;
                addReportUrls(subreports: any): this;
                private _wrapActionProvider;
                initializeFieldListActionProviders(func?: () => Analytics.Internal.IActionsProvider[]): this;
                initializeCalculatedFieldsSource(): this;
                initializeFieldListItemsExtenders(): this;
                initializeParameters(): this;
                initializeFieldListProvider(): this;
                initializeReportItemsProvider(): this;
                initializeDataBindingsProvider(): this;
                initializeDisplayNameProvider(): this;
                initializeExpressionDisplayNameProvider(): this;
                initializeDataSourceHelper(): this;
                addSelection(func?: (settings: Analytics.Internal.SelectionSettings) => void): this;
                addToolboxItems(items?: () => Analytics.Utils.ToolboxItem[]): this;
                addControlProperties(editors: DevExpress.Analytics.Utils.ISerializationInfoArray, groups: DevExpress.Analytics.Internal.GroupObject): this;
                addMenu(func?: (settings: Internal.ReportMenuSettings) => void): this;
                addControlsHelper(func?: (settings: Analytics.Internal.ControlsHelperSettings) => void): this;
                setControlsHelperFilter(filter: (control: any) => boolean): any;
                private _createPropertiesTab;
                private _createExpressionsTab;
                private _createReportExplorerTab;
                private _createFieldListTab;
                addTabPanel(panel?: () => Analytics.Utils.TabPanel, addTabInfo?: () => Analytics.Utils.TabInfo[]): this;
                private _createActionsStorage;
                private _updateCallback;
                addOpenReportMethod(): this;
                initializeUIEffects(applyBindings: boolean, element: Element): this;
                private _createNewViaWizardAction;
                private _createDesignInReportWizardAction;
                private _createMultiQueryDataSourceWizardAction;
                private _customizeDesignerActions;
                addContextActions(func?: (contextActions: Analytics.Internal.ContextActionsSettings) => void): this;
                addActionLists(actionListsFunc?: () => Analytics.Internal.ActionLists): this;
                private _createChartDesignerPart;
                private _createWizardPart;
                addParts(func?: (parts: any) => Analytics.Internal.IDesignerPart[]): this;
                addDefaultAddons(addons?: Analytics.Internal.IDesignerPart[]): this;
                tryAddSqlDataSourceEditorAddon(relationsEditor?: ko.Observable<QueryBuilder.Widgets.Internal.MasterDetailEditor>): this;
                tryAddScriptEditorAddon(isScriptsDisabled: any): this;
                onContextChanged(subreports?: any, func?: (context: IReportDesignerContext) => void): this;
                configurateRtl(rtl: boolean): this;
                configureReportStorageRegistration(reportStorageWebIsRegister: boolean, allowMDI: boolean): this;
                applyUri(uriSettings: IReportUriSettings): this;
                initBindingMode(dataBiningMode: string, convertBindingsToExpressions: string): this;
                registerControls(dataBindingMode: Designer.Utils.DataBindingModeValue, reportItemsProvider: ko.Observable<ReportItemsProvider> | ko.Computed<ReportItemsProvider>): this;
                addCallbacks(callbacks: {
                    designer?: Utils.IReportDesignerCustomizationHandler;
                    preview?: DevExpress.Reporting.Viewer.Utils.IPreviewCustomizationHandler;
                }): this;
                addProcessErrorCallback(processError?: (e: any) => void): this;
                runCustomizeToolboxEvent(customizeToolbox?: (controlsStore: Controls.ControlsFactory) => void): this;
                addLocalization(localization: any): this;
                applyLocalizationToDevExtreme(currentCulture: string): this;
                initCultureInfo(cultureInfoList: any): this;
                updateFont(fontSet: string[] | {
                    [key: string]: string;
                }): this;
                initFormatStringPatterns(formatStringData: any): this;
                addPopularProperties(controlsFactory: any): this;
                addInlineTextEdit(func?: () => Analytics.Internal.InlineTextEdit): this;
                addStylesProjection(styles?: ko.PureComputed<ko.ObservableArray<Controls.StyleModel>>): this;
                addFormattingRulesProjection(rules?: ko.PureComputed<ko.ObservableArray<Controls.FormattingRule>>): this;
                addReportExplorerProvider(reportExplorerProvider?: Analytics.Internal.ObjectExplorerProvider): this;
                addControlsProjection(controlsHelper?: Analytics.Internal.DesignControlsHelper): this;
                addBandsProjection(controlsHelper?: Analytics.Internal.DesignControlsHelper): this;
                addWizards(connectionStrings: Analytics.Wizard.IDataSourceWizardConnectionStrings, wizardSettings: Utils.IReportWizardSettings, cusomizeSettingsFunc: (settings: WizardsInitializerSettings) => void): this;
                addStaticContext(): this;
                tryApplyBindings(applyBindings: boolean, element: Element): this;
                dispose(): void;
                constructor(options: IReportDesignerGeneratorSettings);
            }
        }
        class JSReportDesigner {
            private _designerModel;
            designerModel: IReportDesignerRootContext;
            constructor(_designerModel: ko.Observable<IReportDesignerRootContext>);
            UpdateLocalization(localization: any): void;
            GetDesignerModel(): IReportDesignerRootContext;
            GetPreviewModel(): any;
            GetPropertyInfo(controlType: any, path: any): any;
            GetButtonStorage(): any;
            RunWizard(wizardType: Wizard.WizardRunType): void;
            GetJsonReportModel(): any;
            IsModified(): boolean;
            ResetIsModified(): void;
            AddToPropertyGrid(groupName: any, property: any): void;
            AddParameterType(parameterInfo: any, editorInfo: any): void;
            RemoveParameterType(parameterType: any): void;
            GetParameterInfo(parameterType: any): Data.IParameterTypeValue;
            GetParameterEditor(valueType: any): any;
            ReportStorageGetData(url: any): any;
            ReportStorageSetData(reportLayout: any, url: any): any;
            ReportStorageSetNewData(reportLayout: any, url: any): JQueryPromise<any>;
            SaveReport(): any;
            GetTabs(): Tools.INavigateTab[];
            GetCurrentTab(): Tools.NavigateTab;
            CloseTab(tab: any, force?: boolean): void;
            CloseCurrentTab(): void;
            AdjustControlCore(): void;
            SaveNewReport(reportName: any): JQueryPromise<any>;
            ReportStorageGetUrls(): any;
            OpenReport(url: any): void;
            ShowPreview(): void;
        }
        interface IReportDesignerOptions {
            designerModel?: any;
            initializationData?: Utils.IReportDesignerInitializationModel | ko.Observable<Utils.IReportDesignerInitializationModel>;
            requestOptions?: {
                host: string;
                getDesignerModelAction: string;
                getLocalizationAction?: string;
            };
            callbacks?: {
                designer?: Utils.IReportDesignerCustomizationHandler;
                preview?: DevExpress.Reporting.Viewer.Utils.IPreviewCustomizationHandler;
            };
            reportModel?: any;
            reportUrl?: any;
            parts?: any[];
            limitation?: boolean;
            undoEngine?: any;
        }
        class JSReportDesignerBinding extends DevExpress.Analytics.Internal.JSDesignerBindingCommon<JSReportDesigner> {
            private _initializationData;
            private _callbacks;
            private _model;
            private _deferreds;
            private _applyBindings;
            private _initializeCallbacks;
            private _createModel;
            private _getDesignerModelRequest;
            constructor(_options: IReportDesignerOptions, customEventRaiser?: any);
            dispose(): void;
            applyBindings(element: HTMLElement): void;
        }
    }
}

export default DevExpress;