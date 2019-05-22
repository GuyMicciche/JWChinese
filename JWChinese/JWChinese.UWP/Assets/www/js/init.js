var word;
var leaveTags = ['SCRIPT', 'STYLE', 'TITLE', 'TEXTAREA', 'OPTION'], stripTags = ['WBR'];
function annotPopAll(e) {
    function f(c) {
        var i = 0,
        r = '',
        cn = c.childNodes;
        for (; i < cn.length; i++)
            r += (cn[i].firstChild ? f(cn[i]) : (cn[i].nodeValue ? cn[i].nodeValue : ''));
        return r;
    }

    word = e;

    //V2
    //if (is3Line(e)) {
    //    window.external.notify('ANNOTATION' + '|' + f(e.firstChild.nextSibling.firstChild.firstChild.nextSibling) + '|' + f(e.firstChild) + '|' + e.title)
    //}
    //else {
    //    window.external.notify('ANNOTATION' + '|' + f(e.firstChild.nextSibling.firstChild.firstChild.nextSibling) + '|' + f(e.firstChild) + '|' + e.title)
    //}

    //V1
    if (is3Line(e)) {
        window.external.notify('ANNOTATION' + '|' + f(e.firstChild.nextSibling.firstChild) + '|' + f(e.firstChild.nextSibling.firstChild.nextSibling) + '|' + e.title)
    }
    else {
        window.external.notify('ANNOTATION' + '|' + f(e.firstChild) + '|' + f(e.firstChild.nextSibling) + '|' + e.title)
    }
};
function HTMLSizeChanged(callback) {
    var getLen = function (w) {
        var r = 0;
        if (w.frames && w.frames.length) {
            var i;
            for (i = 0; i < w.frames.length; i++)
                r += getLen(w.frames[i])
        }
        if (w.document && w.document.body && w.document.body.innerHTML)
            r += w.document.body.innerHTML.length;
        return r
    };
    var curLen = getLen(window),
    stFunc = function () {
        window.setTimeout(tFunc, 300)
    },
    tFunc = function () {
        if (getLen(window) == curLen)
            stFunc();
        else
            callback()
    };
    stFunc()
}
function all_frames_docs(c) {
    var f = function (w) {
        if (w.frames && w.frames.length) {
            var i;
            for (i = 0; i < w.frames.length; i++)
                f(w.frames[i])
        }
        c(w.document)
    };
    f(window)
}
function tw0() {
    all_frames_docs(function (d) {
        walk(d, d, false)
    })
}
function annotScan() {
    if (window.document.title != "Chinese") {
        window.external.notify('English Webview loading COMPLETE');
        return;
    }

    tw0();
    all_frames_docs(function (d) {
        if (d.rubyScriptAdded == 1 || !d.body)
            return;
        var e = d.createElement('span');
        //V3
        //e.innerHTML = '<style>ruby {/* font-size: 15pt; */padding-left: 0.1em;padding-right: 0.1em;} ruby rb {line-height: 1.5em;font-size: 1em;} ruby ruby {margin-top: 0;margin-bottom: 0;display: inline-block;line-height: 1.3em;padding: 0;}ruby ruby rt {color:#0070ff;text-align: center;display: block;max-width: 7em;overflow: hidden;text-overflow: ellipsis;white-space: nowrap;font-size: 2em;}ruby ruby rb {display: block;ruby-align: center;text-align: center;font-weight: 600;font-size: 2em}</style>';

        //V2
        e.innerHTML = '<style>ruby{display:inline-table;vertical-align: bottom;padding: .2em;}ruby *{display: inline;line-height:1.0;text-indent:0;text-align:center;white-space:nowrap;}rb{display:table-row-group;font-size: 100%;}rt{display:table-header-group;font-size:100%;line-height:1.1; }</style>';

        //V1
        //e.innerHTML = '<style>ruby{display: inline-table;vertical-align: bottom;padding: .2em;} ruby *{display: inline;text-indent:0;text-align:center;white-space: nowrap;} rb{display: table-row-group;font-size: 1em;/*line-height: 1.5em;*/} rt{display: table-header-group;font-size: 1em;color: #4477a1;}</style>';
        d.body.insertBefore(e, d.body.firstChild);
        var wk = navigator.userAgent.indexOf('WebKit/');
        if (wk > -1 && navigator.userAgent.slice(wk + 7, wk + 12) > 534) {
            var rbs = document.getElementsByTagName('rb');
            for (var i = 0; i < rbs.length; i++) {
                rbs[i].innerHTML = '&#8203;' + rbs[i].innerHTML + '&#8203;'
            }
        }
        d.rubyScriptAdded = 1
    });
    HTMLSizeChanged(annotScan);
    window.external.notify('Chinese Webview annotation COMPLETE');
    //window.JWChinese.getHtml(document.documentElement.innerHTML);
}
function walk(n, document, inLink) {
    var c = n.firstChild;
    while (c) {
        var cNext = c.nextSibling;
        if (c.nodeType == 1 && stripTags.indexOf(c.nodeName) != -1) {
            var ps = c.previousSibling;
            while (c.firstChild) {
                var tmp = c.firstChild;
                c.removeChild(tmp);
                n.insertBefore(tmp, c);
            }
            n.removeChild(c);
            if (ps && ps.nodeType == 3 && ps.nextSibling && ps.nextSibling.nodeType == 3) {
                ps.nodeValue += ps.nextSibling.nodeValue;
                n.removeChild(ps.nextSibling)
            }
            if (cNext && cNext.nodeType == 3 && cNext.previousSibling && cNext.previousSibling.nodeType == 3) {
                cNext.previousSibling.nodeValue += cNext.nodeValue;
                var tmp = cNext;
                cNext = cNext.previousSibling;
                n.removeChild(tmp)
            }
        }
        c = cNext;
    }
    c = n.firstChild;
    while (c) {
        var cNext = c.nextSibling;
        switch (c.nodeType) {
            case 1:
                if (leaveTags.indexOf(c.nodeName) == -1 && c.className != '_adjust0')
                    walk(c, document, inLink || (c.nodeName == 'A' && c.href));
                break;
            case 3: {
                var nv = window.JWChinese.annotate(c.nodeValue);
                if (nv != c.nodeValue) {
                    var newNode = document.createElement('span');
                    newNode.className = '_adjust0';
                    n.replaceChild(newNode, c);
                    newNode.innerHTML = nv;
                }
            }
        }
        c = cNext
    }
}

function englishToggle(def) {
    if (window.document.title != "Chinese") { return; }

    var item = this.word;
    var elements = getAllElementsWithAttributeValue('title', item.getAttribute("title"));

    if (!is3Line(item)) {
        for (var i = 0; i < elements.length; i++) {
            displayEnglish(elements[i], def);
        }
    }
    else {
        for (var i = 0; i < elements.length; i++) {
            hideEnglish(elements[i]);
        }
    }
}

function is3Line(ruby) {
    // V2
    //return ruby.firstChild.nextSibling.firstChild.firstChild.firstChild != null;

    // V1
    return ruby.firstChild.nextSibling.firstChild.childElementCount != null;
}

function displayEnglish(elm, english)
{
    var annotOnly = english.startsWith('*');

    elm.setAttribute('title', english);

    // V2
    //elm.innerHTML = elm.innerHTML.replace("<rt></rt>", "<rt>" + english.split('/')[0] + "</rt>");

    // V1
    //var html = "<rt style='color:#2f64a8;font-size:100%;display:compact;'>";
    //var html = "<rt style='color:#2f64a8;font-size:100%;display:block;max-width:6em;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;text-align:center'>";
    var html;
    if (annotOnly)
    {
        html = "<rt style='color:#2f64a8;font-size:100%;display:none;max-width:6em;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;text-align:center'>";
    }
    else
    {
        html = "<rt style='color:#2f64a8;font-size:100%;display:block;max-width:6em;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;text-align:center'>";
    }

    html += english.split('/')[0];
    html += '</rt><ruby>';
    html += elm.innerHTML;
    html += '</ruby>';

    if (annotOnly)
    {
        html = html.replace(/<rt>/g, "<rt style='display:none'>");
    }

    elm.innerHTML = html;
}

function hideEnglish(elm) {
    // V1
    var html = elm.innerHTML;
    var pattern = /(<rt style)(.*?)(<\/rt>)(<ruby>)(.*?)(<\/ruby>)/g;
    var match = pattern.exec(html);
    html = match[5];

    elm.innerHTML = html.replace(/(<rt style=\'display\:none\'>)/g, "<rt>").replace("display:none", "display:block");


    // V2
    //var html = elm.innerHTML;
    //var pattern = /(<ruby><rt>)(.*?)(<\/rt><rb>)/g;
    //var match = pattern.exec(html);

    //elm.innerHTML = elm.innerHTML.replace(match[2],"");
}


function injectWords(e, p) {
    if (window.document.title != "Chinese") { return; }

    var en = [];
    var pi = [];

    var english = e.split(",");
    var pinyin = p.split(",");
    window.JWChinese.test(english);
    window.JWChinese.test(pinyin);

    // V1
    var rubys = document.getElementsByTagName('ruby');

    // V2
    //var rubys = getAllElementsWithAttribute('ruby', 'title');

    for (var i = 0; i < pinyin.length; i++) {
        var pin = pinyin[i];
        var elements = getAllRTWithNodeValue(rubys, pin)
        for (var j = 0; j < elements.length; j++) {
            displayEnglish(elements[j], english[i]);

            if(!en.contains(english[i]))
            {
                en.push(english[i]);
                pi.push(pinyin[i]);
                window.external.notify('DICTIONARY:' + pinyin[i]);
            }
        }
    }
    writeLine("there are " + en.length + " elements on this page!");

    window.external.notify('Chinese Webview injection COMPLETE');
}

Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

function getAllRTWithNodeValue(elements, value) {
    // V1
    var matchingElements = [];
    for (var i = 0; i < elements.length; i++) {
        if (elements[i].firstChild.nextSibling.firstChild.nodeValue !== null && elements[i].firstChild.nextSibling.firstChild.nodeValue == value)
            matchingElements.push(elements[i]);
    }
    return matchingElements;

    // V2
    //var matchingElements = [];
    //for (var i = 0; i < elements.length; i++) {
    //    if (elements[i].firstChild.firstChild.nodeValue !== null && elements[i].firstChild.firstChild.nodeValue == value)
    //        matchingElements.push(elements[i]);
    //}
    //return matchingElements;
}

function getAllElementsWithAttribute(tag, attribute) {
    var matchingElements = [];
    var allElements = document.getElementsByTagName(tag);
    for (var i = 0, n = allElements.length; i < n; i++) {
        if (allElements[i].getAttribute(attribute) !== null) {
            // Element exists with attribute. Add to array.
            matchingElements.push(allElements[i]);
        }
    }
    return matchingElements;
}

function getAllElementsWithAttributeValue(attribute, value) {
    var matchingElements = [];
    var allElements = document.getElementsByTagName('*');
    for (var i = 0, n = allElements.length; i < n; i++) {
        if (allElements[i].getAttribute(attribute) !== null && allElements[i].getAttribute(attribute) == value) {
            matchingElements.push(allElements[i]);
        }
    }
    return matchingElements;
}

function getHtml() {
    var html = document.documentElement.innerHTML;
    window.JWChinese.getHtml(html);
}

function test(s) {
    window.JWChinese.test(s);
}

window.oncontextmenu = function () {
    window.external.notify('EXPAND');
}

//window.addEventListener("mouseup", mouseClick, false);
window.addEventListener("touchstart", twoFingerTapStart, false);
window.addEventListener("touchend", twoFingerTap, false);

//function mouseClick(event) {
//    window.external.notify('EXPAND');
//    event.preventDefault();
//    return false;
//}

var touches = 0;
function twoFingerTapStart(event) {
    touches++;
}

function twoFingerTap(event) {
    if (touches >= 2) {        
        window.external.notify('EXPAND');
    }
    touches = 0;
    //event.preventDefault();
    return true;
}