var magicPlaylist = (function ($, data) {
    var that;
    var public = {
        init: function () {
            that = private;
            that.rotate.toStart();
            that.deezer.init();
            $('.dz-login').click(that.deezer.login);
        }
    };
    var private = {
        rotate: {
            stepStop: function (showId, hideId, angle, limit, cb) {
                var stop = false;
                var isLimit = limit < 0 ? angle < limit : angle > limit
                if (!stop && isLimit) {
                    $(hideId).addClass('hide');
                    $(showId).removeClass('hide');
                    stop = true;
                    if (cb)
                        cb();
                }
            },
            toStart: function () {
                $('#toInit').rotate({
                    bind: {
                        mouseover: function () {
                            $(this).rotate({
                                animateTo: 360,
                                easing: $.easing.easeInOutElastic,
                                step: function (angle) { that.rotate.stepStop("#toStart", '#toInit', angle, 358) }
                            })
                        }
                    }
                });
            },
            toSuccess: function (cb) {
                $("#toStart").rotate({
                    angle: 0,
                    animateTo: -360,
                    step: function (angle) { that.rotate.stepStop('#toSuccess', '#toStart', angle, -358, cb) }
                });
            },
            toError: function () {
                $("#toStart").rotate({
                    angle: 0,
                    animateTo: -360,
                    step: function (angle) { that.rotate.stepStop('#toError', '#toStart', angle, -358) }
                });
            }
        },
        deezer:{
            init: function () {
                DZ.init({
                    appId: data.apiKey,
                    channelUrl: 'http://localhost:3000/channel'
                });
            },
            login: function () {
                DZ.login(function (response) {
                    if (response.authResponse) {
                        var accessToken = response.authResponse.accessToken
                        console.log('Welcome!  Fetching your information.... ');
                        DZ.api('/user/me', function (response) {
                            response.accessToken = accessToken;
                            console.log('Good to see you, ' + response.name + '.');
                            console.log(response);
                            $.ajax({
                                type: "POST",
                                url: '/playlist',
                                data: response,
                                success: function(result){
                                    that.rotate.toSuccess(function () {
                                        $('.dz-playlist').attr("href", result.playlistUrl);
                                    })
                                },
                                error: that.rotate.toError,
                                dataType: 'json'
                            });
                        });
                    } else {
                        that.rotate.toError()
                        console.log('User cancelled login or did not fully authorize.');
                    }
                }, { perms: 'basic_access,email, manage_library' });
            }
        }
    }    
    return public;
}(jQuery, { apiKey: '170341' }))

$(document).ready(function () {
    magicPlaylist.init();
});