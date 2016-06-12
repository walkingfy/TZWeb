(function ($) {

    //全局系统对象
    window['xcore'] = {};

    //cookies
    xcore.cookies = (function () {
        var fn = function () {
        };
        fn.prototype.get = function (name) {
            var cookieValue = "";
            var search = name + "=";
            if (document.cookie.length > 0) {
                offset = document.cookie.indexOf(search);
                if (offset != -1) {
                    offset += search.length;
                    end = document.cookie.indexOf(";", offset);
                    if (end == -1) end = document.cookie.length;
                    cookieValue = decodeURIComponent(document.cookie.substring(offset, end))
                }
            }
            return cookieValue;
        };
        fn.prototype.set = function (cookieName, cookieValue, DayValue) {
            var expire = "";
            var day_value = 1;
            if (DayValue != null) {
                day_value = DayValue;
            }
            expire = new Date((new Date()).getTime() + day_value * 86400000);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + encodeURIComponent(cookieValue) + ";path=/" + expire;
        }
        fn.prototype.remvoe = function (cookieName) {
            var expire = "";
            expire = new Date((new Date()).getTime() - 1);
            expire = "; expires=" + expire.toGMTString();
            document.cookie = cookieName + "=" + escape("") + ";path=/" + expire;
            /*path=/*/
        };

        return new fn();
    })();

    //预加载图片
    xcore.prevLoadImage = function (rootpath, paths) {
        for (var i in paths) {
            $('<img />').attr('src', rootpath + paths[i]);
        }
    };

    //显示loading
    xcore.showLoading = function (message) {
        message = message || "正在加载中...";
        layer.loading(message);
    };
    //隐藏loading
    xcore.hideLoading = function (loadId) {
        if (loadId != null)
            layer.close(loadId);
        else
            layer.closeAll();
    };

    //封装ajax
    xcore.ajax = function (url, data, callback, isshowerror, dateType, type) {
        var layIndex = null;
        var isShowErrorMsg = isshowerror || true;
        $.ajax({
            type: type || "Post",
            url: url,
            data: data,
            dateType: dateType || "json",
            traditional: true,//阻止深度序列化
            success: function (result) {
                if (callback && typeof (callback) == "function")
                    callback(result);
            },
            error: function (d, c, e) {
                if (isShowErrorMsg == true)
                    xcore.showError("出现错误：" + e);
                if (layIndex != null)
                    layer.close(layIndex);
            },
            beforeSend: function () {
                layIndex = layer.load();
            },
            complete: function () {
                if (layIndex != null)
                    layer.close(layIndex);
            }
        });
    };

    //弹出信息提示框
    xcore.showAlert = function (options, callback) {
        if (options && typeof (options) == "string") {
            var str = options;
            options = {};
            options.msg = str;
        }
        options = $.extend(options, {});
        var size = options.size || "small";
        var titleColor = options.titleColor || "blue";
        var titleIcon = options.titleIcon || "fa-exclamation-circle";
        var titleMsg = options.titleMsg || "提示";
        var msgColor = options.msgColor || "green";
        var msgIcon = options.msgIcon || "fa-comment-to";
        var msgMsg = options.msg || "...";
        //if (callback && typeof (callback) == "function"))
        bootbox.alert({
            local: "zh_CN",
            title: "<span class='" + titleColor + "'><i class='ace-icon fa " + titleIcon + "'></i><strong> " + titleMsg + "</strong></span>",
            message: '<div class="row"><div class="col-xs-12"><i class="ace-icon fa ' + msgIcon + ' bigger-230 ' + msgColor + ' col-xs-2"></i>' +
                '<span class="bigger-125 col-xs-10">' + msgMsg + '</span></div></div>',
            size: size,
            callback: callback || null
        });
    };

    //弹出确认框
    xcore.showConfirm = function (options, callback) {
        if (options && typeof (options) == "string") {
            var str = options;
            options = {};
            options.msg = str;
        }
        options = $.extend(options, {});
        var size = options.size || "small";
        var titleColor = options.titleColor || "blue";
        var titleIcon = options.titleIcon || "fa-question-circle";
        var titleMsg = options.titleMsg || "操作确认";
        var msgColor = options.msgColor || "red";
        var msgIcon = options.msgIcon || "fa-question";
        var msgMsg = options.msg || "确定要执行此操作吗？";
        //if (callback && typeof (callback) == "function"))
        bootbox.dialog({
            local: "zh_CN",
            title: "<span class='" + titleColor + "'><i class='ace-icon fa " + titleIcon + "'></i><strong> " + titleMsg + "</strong></span>",
            message: '<div class="row"><div class="col-xs-12"><i class="ace-icon fa ' + msgIcon + ' bigger-230 ' + msgColor + ' col-xs-2"></i>' +
                '<span class="bigger-125 col-xs-10">' + msgMsg + '</span></div></div>',
            size: size,
            buttons: {
                b1: {
                    label: "确定",
                    className: "btn-danger",
                    icon: "fa-trash-o",
                    callback: function (e, dialog) {
                        if (callback && typeof (callback) == "function")
                            callback(dialog);
                        return true;
                    }
                },
                b2: {
                    label: "取消",
                    className: "btn-default",
                    icon: "fa-close",
                    callback: function (e, dialog) {
                        return true;
                    }
                }
            }
        });
    };

    //显示成功提示窗口
    xcore.showSuccess = function (message, callback) {
        if (typeof (message) == "function" || arguments.length == 0) {
            callback = message;
            message = "操作成功!";
        }

        xcore.showAlert({
            titleColor: "blue",
            titleIcon: "fa-exclamation-circle",
            titleMsg: "提示",
            msgColor: "green",
            msgIcon: "fa-check",
            msg: message
        }, callback || null);
    };

    //显示成功消息从右下角
    xcore.showSuccessBottomRight = function (message, callback) {
        var notify = new PNotify({
            title: '信息提示',
            text: message || "操作成功",
            type: "success",
            addclass: "stack-bottomright",
            stack: { "dir1": "up", "dir2": "left", "firstpos1": 25, "firstpos2": 25 },
            delay: 1000, //消失时间
            buttons: {
                sticker: false
            }
        });
        notify.get().click(function () {
            notify.remove();
        });
        //因为Pnotify显示不会阻塞脚本，所以执行回调函数
        if (callback && typeof (callback) == "function")
            callback();
    };

    //显示失败提示窗口
    xcore.showError = function (message, callback) {
        if (typeof (message) == "function" || arguments.length == 0) {
            callback = message;
            message = "操作失败!";
        }

        xcore.showAlert({
            titleColor: "red",
            titleIcon: "fa-exclamation-circle",
            titleMsg: "错误",
            msgColor: "red",
            msgIcon: "fa-close",
            msg: message
        }, callback || null);
    };

    //显示操作消息
    xcore.showOperationMsg = function (operationResult, showStyle, successCallback) {
        if (operationResult.ResultType == "Error") {
            xcore.showAlert(operationResult.Message);
        } else {
            if (showStyle == "alert")
                xcore.showSuccess(operationResult.Message, successCallback);
            else if (showStyle == "notify")
                xcore.showSuccessBottomRight(operationResult.Message, successCallback);
        }
    };

    xcore.showHtml = function (options, callback) {
        options = $.extend(options, {});
        var url = options.url;
        var width = options.width || 500;
        var height = options.height || 300;
        var title = options.title;
        //显示Loading效果
        var layIndex = layer.load();
        $.get(url, function (datas) {
            var modal = bootbox.dialog({
                title: "<span class='blue'><i class='ace-icon fa info-circle'></i><strong> " + title + "</strong></span>",
                message: datas,
                size: "custom",
                width: width,
                height: height,
                showOnLoad: function (dialog) {
                    //关闭loading效果
                    if (layIndex != null)
                        layer.close(layIndex);
                    callback(dialog);
                }
            });
        });
    };

    //显示html
    xcore.showEditHtml = function (options, callback) {
        options = $.extend(options, {});
        var url = options.url;
        var saveurl = options.saveurl;
        var width = options.width || 500;
        var height = options.height || 300;
        var titleIcon = options.titleIcon;
        var title = options.title;
        var closeCallBack = options.closeCallBack;
        //显示Loading效果
        var layIndex = layer.load();
        $.get(url, function (datas) {
            var modal = bootbox.dialog({
                title: "<span class='blue'><i class='ace-icon fa " + titleIcon + "'></i><strong> " + title + "</strong></span>",
                message: datas,
                size: "custom",
                width: width,
                height: height,
                showOnLoad: callback,
                buttons: {
                    b1: {
                        label: "确定",
                        className: "btn-primary",
                        icon: "fa-check",
                        callback: function (e, dialog) {
                            var form = $("form", dialog);


                            // 要调用 valid()方法。
                            if (form.valid(form, "") == false) {
                                return false;
                            }

                            //$(this).button('loading');
                            var entity = xcore.getFields(form);
                            xcore.ajax(saveurl, entity, function (result) {
                                xcore.showOperationMsg(result, "alert", function () {
                                    //执行刷新事件
                                    if (closeCallBack && typeof (closeCallBack) == "function")
                                        closeCallBack();
                                });
                            });
                            return false;
                        }
                    },
                    b2: {
                        label: "取消",
                        className: "btn-default",
                        icon: "fa-close",
                        callback: function (e, dialog) {
                            return true;
                        }
                    }
                }
            });
            //关闭loading效果
            if (layIndex != null)
                layer.close(layIndex);
        });
    };
    //赋值字段
    xcore.setFields = function (entity, target, selector) {
        ///	<summary>
        ///	初始化页面
        /// 多实体格式 {entity1:{field1:123,field2:234},entity2:{fiel:123,fiel2:123},otherField:"abc"}
        /// HTML:<input field="entity1.field1" /><input field="entity2.fiel" />...
        /// 支持多选,多选返回[]
        ///	</summary>
        ///	<param name="entity" type="Object">
        /// 实体内容
        ///	</param>
        ///	<param name="target" type="DOM">
        /// 页面区域
        ///	</param>
        ///	<param name="selector" type="String">
        /// JQuery选择器
        ///	</param>
        if (entity == undefined)
            return;
        if (target == undefined)
            target = this._container;
        selector = selector || 'field';
        var allfileds = $("[" + selector + "]", target);
        var a = "";

        allfileds.each(function (i) {
            var element = $(this);
            var key = element.attr(selector);

            var tokens = key.toString().split('.');
            var val;
            if (tokens != undefined && tokens.length && tokens.length > 1) {
                var ent = entity[tokens[0]];
                if (ent != undefined)
                    val = ent[tokens[1]];
            } else {
                val = entity[key];
            }

            if (val != undefined) {

                if (val.constructor && val.constructor == Date)
                    val = val.toString("yyyy-MM-dd");

                if (element.attr("type") == "radio") {
                    $("[name='" + element.attr("name") + "']", target).each(function () {
                        if ($(this).val() == val)
                            $(this).prop("checked", true);
                        else
                            $(this).prop("checked", false);
                    });
                } else if (element.attr("type") == "checkbox") {
                    var dom = $("[name='" + element.attr("name") + "']", target);
                    if (dom.length > 1) {
                        $.each(val, function (index) {
                            var thatelemetn = $("[name='" + element.attr("name") + "'][value='" + val[index] + "']", target);
                            thatelemetn.prop("checked", true);
                        });
                    } else {
                        //对普通checkbox和bootstrap_switch分别进行处理
                        if (element.attr("ctype") == "switch") {
                            element.bootstrapSwitch('state', val);
                        } else
                            element.prop("checked", val);

                    }
                } else if (!(element.is('input') || element.is('textarea') || element.is('select'))) {
                    element.text(val);
                } else {
                    element.val(val);
                }
            }
        });
    };
    //取值字段
    xcore.getFields = function (target) {
        ///	<summary>
        ///	根据属性field 从指定的页面区域获取内容 , 返回实体 :{NavId="...",NavName=".."}
        /// 多实体格式 {entity1:{field1:123,field2:234},entity2:{fiel:123,fiel2:123},otherField:"abc"}
        /// 对选支持:格式[]
        ///	</summary>
        ///	<param name="target" type="DOM">
        /// 页面区域
        ///	</param>
        ///	<return>
        ///	实体内容
        ///	</return>

        var entity = {};
        if (target == undefined)
            target = this._container;

        var allfileds = $("[field]", target);

        allfileds.each(function (i) {
            var element = $(this);
            var key = element.attr("field");

            var tokens = key.toString().split('.');


            var tokevalue = function (v) {
                ///	<summary>
                ///	实体赋值
                ///	</summary>
                if (tokens != undefined && tokens.length && tokens.length > 1) {
                    if (entity[tokens[0]] == undefined)
                        entity[tokens[0]] = {};
                    entity[tokens[0]][tokens[1]] = v;
                }
                else {
                    entity[key] = v;
                }
            };
            //根据元素不同做不同处理
            if (element.attr("type") == "radio") {
                $("[name='" + element.attr("name") + "']", target).each(function () {
                    var r = $(this);
                    if (r.attr("checked")) {
                        tokevalue(r.val());
                    }
                });
            } else if (element.attr("type") == "checkbox") {
                if ($("[name='" + element.attr("name") + "']", target).length > 1) {
                    var last = [];
                    $("[name='" + element.attr("name") + "']", target).each(function () {
                        var r = $(this);
                        if (r.attr("checked")) {
                            last.push(r.val());
                        }
                    });
                    tokevalue(last);
                } else {
                    tokevalue($(this).attr("checked") == "checked");
                }
            }
            else {
                if (element.attr("ctype") == "magicSelect") {
                    var last = [];
                    $("[field='" + element.attr("name") + "']", target).each(function () {
                        var r = $(this);
                        last.push(r.val());
                    });
                    tokevalue(last);
                }
                else
                    tokevalue($(this).val());
            }
        });
        return entity;
    };
    //刷新jqGrid
    xcore.JqGridReLoad = function (jqGrid) {
        jqGrid.trigger("reloadGrid");
    };
    //初始化jqGrid
    xcore.JqGrid = function (options) {
        options = $.extend(options, {});
        var gridSelector = options.gridSelector;
        var pagerSelector = options.pagerSelector;
        if ($(gridSelector).length > 0 && $(pagerSelector).length > 0) {
            var jGrid = jQuery(gridSelector).jqGrid({
                url: options.url,
                datatype: 'json',
                mtype: "POST",
                height: "auto",
                colNames: options.colNames,
                colModel: options.colModel,
                viewrecords: true,
                rowNum: 10,
                rowList: [10, 20, 30],
                pager: pagerSelector,
                altRows: true,
                autowidth: true,
                shrinkToFit: true,
                forceFit: true,
                treeGrid: options.treeGrid,
                treeGridModel: options.treeGridModel,
                treedatatype: "json",
                treeIcons:options.treeIcons,
                treeReader: {
                    level_field: "level",
                    parent_id_field: "parent",
                    leaf_field: "isLeaf",
                    expanded_field: "expanded"
                },
                ExpandColumn: options.ExpandColumn,
                multiselect: options.multiselect,
                loadComplete: options.loadComplete,
                caption: options.caption
            });
            return jGrid;
        }
    };
    //获取选择的jqGrid行
    xcore.GetSelectJqGridRow = function (jqGrid) {
        var rowId = jqGrid.jqGrid('getGridParam', 'selrow');
        if (rowId && rowId != "") {
            var rowData = jqGrid.jqGrid('getRowData', rowId);
            return rowData;
        } else {
            xcore.showError("请至少选择一行");
            return null;
        }
    };
    //新增jqGrid
    xcore.showAddPage = function (jqGrid, options) {
        options = $.extend(options, {});
        xcore.showEditHtml({
            url: options.url || "Edit",
            saveurl: options.saveurl || "Create",
            width: options.width || 500,
            height: options.height || 300,
            titleIcon: "fa-plus",
            title: "新增",
            closeCallBack: function () {
                xcore.JqGridReLoad(jqGrid);
            }
        });
    };
    //修改jqGrid
    xcore.showUpdatePage = function (jqGrid, options) {
        options = $.extend(options, {});
        var rowData = xcore.GetSelectJqGridRow(jqGrid);
        if (rowData) {
            xcore.showEditHtml({
                url: options.url || "Edit",
                saveurl: options.saveurl || "Update",
                width: options.width || 500,
                height: options.height || 300,
                titleIcon: "fa-edit",
                title: "修改",
                closeCallBack: function () {
                    xcore.JqGridReLoad(jqGrid);
                }
            }, function (dialog) {
                xcore.setFields(rowData, dialog);
            });
        }
    }
    //修改jqGrid的IsVisible
    xcore.showDelete = function (jqGrid, options) {
        options = $.extend(options, {});
        var rowData = xcore.GetSelectJqGridRow(jqGrid);
        if (rowData) {
            rowData.IsVisible = false;
            xcore.showConfirm("确定要执行删除操作吗？", function () {
                xcore.ajax(options.url || "Update", rowData, function (result) {
                    xcore.showOperationMsg(result, "notify");
                    xcore.JqGridReLoad(jqGrid);
                });
            });
        }
    }
    //删除jqGrid数据
    xcore.showRealDelete = function (jqGrid, options) {
        options = $.extend(options, {});
        var rowData = xcore.GetSelectJqGridRow(jqGrid);
        if (rowData) {
            xcore.showConfirm("确定要执行删除操作吗？删除后数据不可恢复", function () {
                xcore.ajax(options.url || "Delete", rowData, function (result) {
                    var rowid = jqGrid.getGridParam("selrow");
                    jqGrid.jqGrid("delRowData", rowid);
                    xcore.showOperationMsg(result, "notify");
                    xcore.JqGridReLoad(jqGrid);
                });
            });
        }
    }

    //initForm
    xcore.initForm = function (form, isValidateForm) {
        $('input[ctype="switch"]', form).bootstrapSwitch();

        $('input[ctype="switch"]', form).on('switchChange.bootstrapSwitch', function (event, state) {
            if (state == true)
                $(this).attr("checked", "checked");
            else
                $(this).removeAttr("checked");
        });

        if (isValidateForm == true)
            form.validation();//reqmark不设置*号
    }

})(jQuery);


//Jquery扩展

//获取URL参数
$.extend({
    getUrlParam: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    }
});


//定义Tree所需类
var DataSourceTree = function (options) {
    this._data = options.data;
    this._delay = options.delay;
}

DataSourceTree.prototype.data = function (options, callback) {
    var self = this;
    var $data = null;

    if (!("name" in options) && !("type" in options)) {
        $data = this._data;//the root tree
        callback({ data: $data });
        return;
    }
    else if ("type" in options && options.type == "folder") {
        if ("additionalParameters" in options && "children" in options.additionalParameters)
            $data = options.additionalParameters.children;
        else $data = {}//no data
    }

    if ($data != null)//this setTimeout is only for mimicking some random delay
        setTimeout(function () { callback({ data: $data }); }, parseInt(Math.random() * 500) + 200);
};