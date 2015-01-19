(function (window) {
    var initializing = false, fnTest = /xyz/.test(function () {
        xyz
    }) ? /\b_super\b/ : /.*/;
    this.jsClass = function () {
    };
    jsClass.extend = function (prop) {
        var _super = this.prototype;
        initializing = true;
        var prototype = new this;
        initializing = false;
        for (var name in prop) {
            prototype[name] = typeof prop[name] == "function" && typeof _super[name] == "function" && fnTest.test(prop[name]) ? function (name, fn) {
                return function () {
                    var tmp = this._super;
                    this._super = _super[name];
                    var ret = fn.apply(this, arguments);
                    this._super = tmp;
                    return ret
                }
            } (name, prop[name]) : prop[name]
        }
        function jsClass() {
            if (!initializing && this.init) {
                this.init.apply(this, arguments)
            }
        }
        jsClass.prototype = prototype;
        jsClass.prototype.constructor = jsClass;
        jsClass.extend = arguments.callee;
        return jsClass
    };

    this.namespace = function (koPath, object) {
        var tokens = koPath.split("."), target = window;
        for (var i = 0; i < tokens.length - 1; i++) {
            target[tokens[i]] = target[tokens[i]] || {};
            target = target[tokens[i]];
        }
        target[tokens[tokens.length - 1]] = object;      
    };

})(window);

Array.prototype.ForEach = function (callback) {
    for (var i = 0, len = this.length; i < len; i++) {
        callback(this[i], i);
    }
};
Array.prototype.remove = function (callback) {
    var fx = function (arr) { return arr.length; };
    for (var i = 0; i < fx(this); i++) {
        if (callback(this[i])) { this.splice(i, 1); i--; }
    }
    return this;
};
Array.prototype.Where = function (callback) {
    var res = [];
    for (var i = 0, len = this.length; i < len; i++) {
        if (callback(this[i])) { res.push(this[i]); }
    }
    return res;
};

Array.prototype.select = function (conditionalCallback, selectionCallback) {
    var res = [];
    for (var i = 0, len = this.length; i < len; i++) {
        if (conditionalCallback(this[i])) { res.push(selectionCallback(this[i])); }
    }
    return res;
};
Array.prototype.First = function (callback) {
   
    for (var i = 0, len = this.length; i < len; i++) {
        if (callback(this[i])) { return this[i]; }
    }
    return null;
};
Array.prototype.Find = Array.prototype.First;
Array.prototype.Last = function (callback) {
    var obj = null;
    for (var i = 0, len = this.length; i < len; i++) {
        if (callback(this[i])) { obj= this[i]; }
    }
    return obj;
};
Array.prototype.FindLast = Array.prototype.Last;

Array.prototype.selectWithJoin = function (list1, list1Callback, conditionalCallback, selectionCallback) {
    var res = [], fx = function (list, pitem, callback) {
        var temp = [];
        for (var i = 0, len = list.length; i < len; i++) {
            if (callback(pitem, list[i])) { temp.push(list[i]); }
        }
        return temp;
    };
    this.ForEach(function (item) {
        fx(list1, item, list1Callback).ForEach(function (join) {
            if (conditionalCallback(item, join)) {
                res.push(selectionCallback(item, join));
            }
        });
    });
    return res;
};
Array.prototype.groupBy = function (callback) {
    var res = [], temp = [];
    this.ForEach(function (item) {
        var key = callback(item);
        if (temp[key]) {
            temp[key].push(item);
        } else {
            temp[key] = [item];           
        }
    });
    for (var i in temp) {
        if (temp.hasOwnProperty(i)) {           
            res.push({ key: i, items: temp[i] });
        }
    }
    return res;
};
Array.prototype.paging = function (pageno, size) {
    pageno--;
    return this.slice(pageno * size, (pageno * size) + size);
};

Object.defineProperty(Array.prototype, 'Count', { get: function () {
    return this.length;
}
});
Object.defineProperty(String.prototype, 'Length', { get: function () {
    return this.length;
}
});
Array.prototype.Insert = function (index, item) {
    return this.splice(index, 0, item);
}
Array.prototype.Add = function (item) { return this.push(item); };
Array.prototype.Join = function (str) { return this.join(str); };
String.prototype.StartsWith = function (str) {
    return (this.indexOf(str) == 0);
};
String.prototype.Substring = function (a, b) { return this.substring(a, b); };
String.prototype.ToCharArray = function () { return this; };
String.prototype.ToLower = function () { return this.toLowerCase(); };
String.prototype.ToUpper = function () { return this.toUpperCase(); };
String.prototype.Trim = function () { return this.trim(); };
String.prototype.Split = function (str) { return this.split(str); };
String.prototype.Replace = function (oldStr, newStr) { return this.replace(oldStr, newStr); };
String.prototype.IndexOf = function (str) { return this.indexOf(str); };
String.prototype.LastIndexOf = function (str) { return this.lastIndexOf(str); };
String.prototype.format = function () {
    var str = this;
    for (var i = 0; i < arguments.length; i++) {
        str = str.replace(new RegExp('\\{' + i + '\\}', 'g'), arguments[i]);
    }
    return str;
};

function GetParentScope(scope) {
    return scope.$parent;
}
function GetScopeRoot(scope) {
    return scope.$root;
}
function SetScopePropValue(scope, prop, value) {
    scope[prop] = value;
}

var SQLight = jsClass.extend({
    init: function () {
        var ops = SQLight.dbOptions;
        this.db = openDatabase(ops.fileName, ops.version, ops.displayName, ops.maxSize);
    },
    query: function (sql, paramsArray) {
        var res = {}, dbRef = this.db, scb = null, ecb = null;
        res.init = function () {
            dbRef.transaction(function (t) {
                t.executeSql(sql, paramsArray||[], function (t, res) { if (scb) { scb(res, t); } }, function (t, res) { if (ecb) { ecb(res, t); } });
            });
            return res;
        };
        res.success = function (callback) {
            scb = callback;
            return res;
        };
        res.error = function (callback) {
            ecb = callback;
            return res;
        };
        return res.init();
    }
});
SQLight.setDbOptions = function (fileName, version, displayName, maxSize) {
    SQLight.dbOptions = { fileName: fileName, version: version, displayName: displayName, maxSize: maxSize };
};
SQLight.getList = function (res) {
    var arr = [];
    for (var i = 0, len = res.rows.length; i < len; i++) {
        arr.push(res.rows.item(i));
    }
    return arr;
};

var Dictionary = jsClass.extend({

    Add: function (key, value) {
        this[key] = value;
    },
    ContainsKey: function (key) {
        return !!this[key];
    },
    ContainsValue: function (value) {
        for (var item in this) {
            if (this.hasOwnProperty(item) && this[item] == value) return true;
        }
        return false;
    },
    ToList: function () {
        var kvList = [];
        for (var item in this) {
            if (this.hasOwnProperty(item)) { kvList.push({ Key: item, Value: this[item] }); }
        }
        return kvList;
    },
    Where: function (callback) {
        return this.ToList().Where(callback);
    },
    ForEach: function (callback) {
        this.ToList().ForEach(callback);
    },
    Remove: function (key) {
        if (this.hasOwnProperty(key)) {
            delete this[key];
        }
    },
    ToJsonObject: function () {
        var res = {};
        for (var item in this) {
            if (this.hasOwnProperty(item)) { res[item] = this[item]; }
        }
        return res;
    },
    Clear: function () {
        for (var item in this) {
            if (this.hasOwnProperty(item)) { delete this[item]; }
        }
    }
});
Object.defineProperty(Dictionary.prototype, 'Keys', { get: function () {
    var temo = [];
    for (var key in this) {
        if (this.hasOwnProperty(key)) { temo.push(key); }
    }
    return temo;
}
});
Object.defineProperty(Dictionary.prototype, 'Values', { get: function () {
    var temo = [];
    for (var key in this) {
        if (this.hasOwnProperty(key)) { temo.push(this[key]); }
    }
    return temo;
}
});
Object.defineProperty(Dictionary.prototype, 'Count', { get: function () {
    var count = 0;
    for (var key in this) {
        if (this.hasOwnProperty(key)) { count++; }
    }
    return count;
}
});
