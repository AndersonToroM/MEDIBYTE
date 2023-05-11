/**
 * DevExtreme (core/utils/storage.js)
 * Version: 19.1.6
 * Build date: Wed Sep 11 2019
 *
 * Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
 * Read about DevExtreme licensing here: https://js.devexpress.com/Licensing/
 */
"use strict";
var window = require("../../core/utils/window").getWindow();
var getSessionStorage = function() {
    var sessionStorage;
    try {
        sessionStorage = window.sessionStorage
    } catch (e) {}
    return sessionStorage
};
exports.sessionStorage = getSessionStorage;
