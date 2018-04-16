(function (window) {
    var $ = window.jQuery || window.me || (window.me = {}),
        throttle = function (fn, timeout, callback, delayed, trailing, debounce) {
            timeout || (timeout = 100);
            var timer = false,
                lastCall = false,
                hasCallback = (typeof callback === "function"),
                startTimer = function (wrapper, args) {
                    timer = setTimeout(function () {
                        timer = false;
                        if (delayed || trailing) {
                            fn.apply(wrapper, args);
                            if (trailing) { lastCall = +new Date(); }
                        }
                        if (hasCallback) { callback.apply(wrapper, args); }
                    }, timeout);
                },
                wrapper = function () {
                    if (timer && !debounce) { return; }
                    if (!timer && !delayed) {
                        if (!trailing || (+new Date() - lastCall) > timeout) {
                            fn.apply(this, arguments);
                            if (trailing) { lastCall = +new Date(); }
                        }
                    }
                    if (debounce || !trailing) { clearTimeout(timer); }
                    startTimer(this, arguments);
                }
            if ($.guid) { wrapper.guid = fn.guid = fn.guid || $.guid++; }
            return wrapper;
        };
    $.throttle = throttle;
    $.debounce = function (fn, timeout, callback, delayed, trailing) {
        return throttle(fn, timeout, callback, delayed, trailing, true);
    };
})(this);

function isIEorEDGE(){
  return navigator.appName == 'Microsoft Internet Explorer' || (navigator.appName == "Netscape" && navigator.appVersion.indexOf('Edge') > -1);
}


var PullToRefresh = (function () {
    function Main(container, slidebox, slidebox_icon, _IEEDGE_SHOW_BUTTON) {

        var self = this;

        this.breakpoint = 80;

        this._container = container;
        this.container = container[0];

        this.slidebox = slidebox[0];
        this.slidebox_icon = slidebox_icon[0];
        this.handler = null;

        this.scrollBox = $('.page-twitter ul');
        this.scrollBoxHeight = this.scrollBox.height();

        this._slidedown_height = 0;
        this._anim = null;
        this._dragged_down = false;

        this._isEdge = isIEorEDGE() && !_IEEDGE_SHOW_BUTTON;

        if (this._isEdge) {
            $('#refresh-control').addClass('t__hide');
            this.scrollBox.css('touch-action', 'none');
        }

        this.scrollBox.scroll(function(ev) {
            self.scrollDown(self, ev);
        });

        this.hammertime = Hammer(this.container)
            .on("touch dragdown dragup release", function(ev) {
                self.handleHammer(ev);
            });
    }

    Main.prototype.handleHammer = function(ev) {
        var self = this;

        switch(ev.type) {

            case 'touch':
                this.hide();
                break;


            case 'release':
                if(!this._dragged_down) {
                    return;
                }


                cancelAnimationFrame(this._anim);


                if(ev.gesture.deltaY >= this.breakpoint) {
                    self._container.addClass('pullrefresh-loading');
                    self.slidebox_icon.className = 'icon ticon-loader';

                    this.setHeight(60);
                    this.handler.call(this);
                }

                else {
                    self.slidebox.className = 'slideup';
                    self._container.addClass('pullrefresh-slideup');

                    this.hide();
                }
                break;

            case 'dragup':
                var self = this;
                this.edgeScrollUp(self);
                break;

            case 'dragdown':

                var self = this;
                this.edgeScrollDown(self);


                var scrollY = this.scrollBox.scrollTop();
                if(scrollY > 5) {
                    return;
                } else if(scrollY !== 0) {
                    this.scrollBox.scrollTo(0,0);
                }

                this._dragged_down = true;

                if(!this._anim) {
                    this.updateHeight();
                }

                ev.gesture.preventDefault();

                this._slidedown_height = ev.gesture.deltaY * 0.4;
                break;
        }
    };


    Main.prototype.setHeight = function (height) {

        if (height === 0) {
            this._container.removeClass('no-scroll');
        } else {
            this._container.addClass('no-scroll');
        }

        if (Modernizr.csstransforms3d) {

            this.container.style.transform = 'translate3d(0,' + height + 'px,0) ';
            this.container.style.oTransform = 'translate3d(0,' + height + 'px,0)';
            this.container.style.msTransform = 'translate3d(0,' + height + 'px,0)';
            this.container.style.mozTransform = 'translate3d(0,' + height + 'px,0)';
            this.container.style.webkitTransform = 'translate3d(0,' + height + 'px,0) scale3d(1,1,1)';
        }
        else if (Modernizr.csstransforms) {
            this.container.style.transform = 'translate(0,' + height + 'px) ';
            this.container.style.oTransform = 'translate(0,' + height + 'px)';
            this.container.style.msTransform = 'translate(0,' + height + 'px)';
            this.container.style.mozTransform = 'translate(0,' + height + 'px)';
            this.container.style.webkitTransform = 'translate(0,' + height + 'px)';
        }
        else {
            this.container.style.top = height + "px";
        }
    };

    Main.prototype.hide = function () {
        this._container.removeClass('pullrefresh-slideup pullrefresh-loading pullrefresh-breakpoint');
        this._slidedown_height = 0;
        this.setHeight(0);
        cancelAnimationFrame(this._anim);
        this._anim = null;
        this._dragged_down = false;
    };

    Main.prototype.slideUp = function () {
        var self = this;
        cancelAnimationFrame(this._anim);

        self.slidebox.className = 'slideup';
        self._container.addClass('pullrefresh-slideup');

        this.setHeight(0);

        setTimeout(function () {
            self.hide();
        }, 500);
    };

    Main.prototype.updateHeight = function () {
        var self = this;

        this.setHeight(this._slidedown_height);

        if (this._slidedown_height >= this.breakpoint) {
            this.slidebox.className = 'breakpoint';
            this.slidebox_icon.className = 'icon arrow arrow-up';
        }
        else {
            this.slidebox.className = '';
            this.slidebox_icon.className = 'icon arrow';
        }

        this._anim = requestAnimationFrame(function () {
            self.updateHeight();
        });
    };

    Main.prototype.scrollDown = $.debounce(scrollDown, 100, null, true);

    Main.prototype.edgeScrollUp = $.debounce(edgeScrollUp, 100, null);
    Main.prototype.edgeScrollDown = $.debounce(edgeScrollDown, 100, null);

    function edgeScrollUp(self) {
        if (self._isEdge) {
            var scrollY = self.scrollBox.scrollTop();
            self.scrollBox.animate({ scrollTop : (scrollY + 200)}, 200);
        }
    }

    function edgeScrollDown(self) {
        if (self._isEdge) {
            var scrollY = self.scrollBox.scrollTop();
            if (scrollY < 200) {
                self.scrollBox.animate({ scrollTop : 0}, 200);
            } else {
                self.scrollBox.animate({ scrollTop : (scrollY - 200)}, 200);
            }
        }
    }

    function scrollDown(self, ev) {
        var scrollY = self.scrollBox.scrollTop();
        var scrollHeight = self.scrollBox[0].scrollHeight;
        if ((self.scrollBoxHeight + scrollY + 150) > scrollHeight) {
            var bridgeService = new OpenMVVM.BridgeService();
            bridgeService.fireCommand("MainViewModel.PageItems[3].GetOlderTweetsCommand", "");
        }
    }

    return Main;
})();

(function () {

    var _ANIMATE_SCROLL = false;
    var _IEEDGE_SHOW_BUTTON = true;

    var views = {};

    var scrollEffects = function (ev) {
        var el = $(ev.target);
        var top = el.scrollTop();

        headerEffects(top);

        var subview = el.attr('subview');
        if (subview) {
            views['MainView'].subviews[subview].scrollPos = top;
        }
    };

    var headerEffects = function (scrollPos) {
        if (scrollPos > 0) {
            $('body header').addClass('hover');
        } else {
            $('body header').removeClass('hover');
        }
    }

    var scrollDebounced = $.debounce(scrollEffects, 100, null, true);

    var setupSubview = function (viewEl, scrollPos) {

        var scroll;

        if (scrollPos !== undefined) {
            scroll = scrollPos;
        } else {
            var subview = views['MainView'].subviews[viewEl.attr('subview')];
            scroll = subview ? subview.scrollPos : 0;
        }
        _ANIMATE_SCROLL ? viewEl.animate({ scrollTop: scroll }, 200) : viewEl.scrollTop(scroll);

        headerEffects(scroll);
    }

    var setupView = function (viewEl, scrollPos) {

        _ANIMATE_SCROLL ? viewEl.animate({ scrollTop: scrollPos }, 200) : viewEl.scrollTop(scrollPos);

        headerEffects(scrollPos);
    }

    OpenMVVM.on('SubviewChange', function (data) {

        if (!views['MainView'].subviews[data.newVal]) {
            views['MainView'].subviews[data.newVal] = { scrollPos: 0 };
        }

        setTimeout(function () {

            //setup el
            var view = $(data.el).find('.view:visible');
            view.attr('subview', data.newVal);

            //refresh scroll
            setupSubview(view, 0);

        }, 100);

    });

    OpenMVVM.on('ViewChange',
        function(data) {

            if (views[data.next] === undefined) {
                views[data.next] = {};
            }

            switch (data.next) {
            case 'MainView':
                if (!views[data.next].loaded) {

                    views[data.next].subviews = {};

                    $('.view').each(function(i, val) {

                        $(val).scroll(function(ev) {
                            scrollDebounced(ev);
                        });
                    });

                    var refresh = new PullToRefresh($('.page-twitter'), $('#pullrefresh'), $('#pullrefresh-icon'), _IEEDGE_SHOW_BUTTON);

                    var bridgeService = new OpenMVVM.BridgeService();

                    // update image onrefresh
                    refresh.handler = function() {
                        var self = this;
                        bridgeService.fireCommand("MainViewModel.PageItems[3].RefreshTweetsCommand", "");
                        setTimeout(function() {
                                self.slideUp();
                            },
                            500);
                    };

                    views[data.next].loaded = true;
                }
                setupSubview($('.view:visible'));

                break;
            case 'FavoritesView':
            case 'SessionView':
            case 'SpeakerView':
                if (!views[data.next].loaded) {

                    $('.view').scroll(function(ev) {
                        scrollDebounced(ev);
                    });

                    views[data.next].loaded = true;
                }
                setupView($('.view:visible'));
                break;
            }
        });

    OpenMVVM.jsBind();
})();
