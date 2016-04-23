var magicPlaylist = (function ($, data) {
    return {
        rotate: {
            start: function (className, cb) {

                var stepFcn = function (angle) {
                    var stop = false;
                    if (!stop && angle > 358) {
                        $('#first').addClass('hide');
                        $('#second').removeClass('hide');
                        stop = true;
                    }
                }

                var mouseOverFcn = function (step) {
                    return function () {
                        $(this).rotate({
                            animateTo: 360,
                            easing: $.easing.easeInOutElastic,
                            step: step
                        })
                    }
                }

                $('.' + className).rotate({
                    bind: {
                        mouseover: mouseOverFcn(stepFcn)
                    }
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
                                success: function (result) {
                                    if (result.success) {
                                        var stepFcn = function (angle) {
                                            var stop = false;
                                            if (!stop && angle < -358) {
                                                $('#second').addClass('hide');
                                                $('#third').removeClass('hide');
                                                stop = true;
                                            }
                                        }

                                        $(".logo2").rotate({
                                            angle: 0,
                                            animateTo: -360,
                                            step: stepFcn
                                        });
                                        
                                    }
                                },
                                dataType: 'json'
                            });

                            
                        });
                    } else {
                        console.log('User cancelled login or did not fully authorize.');
                    }
                }, { perms: 'basic_access,email, manage_library' });
            }
        }
    }
}(jQuery, { apiKey: '170341' }))

$(document).ready(function () {
    magicPlaylist.rotate.start("logo");
    magicPlaylist.deezer.init();
    $('.dz-login').click(magicPlaylist.deezer.login);
});

//$(".logo")
//	  	.rotate({
//	  	    bind: {
//	  	        mouseover: function () {
//	  	            $(this).rotate({
//	  	                animateTo: 360,
//	  	                easing: $.easing.easeInOutElastic,
//	  	                callback: function () {

//	  	                },
//	  	                step: function (angle) {
//	  	                    var stop = false;
//	  	                    if (!stop && angle > 358) {
//	  	                        $('#first').addClass('hide');
//	  	                        $('#second').removeClass('hide');
//	  	                        stop = true;
//	  	                    }

//	  	                }
//	  	            })
//	  	        }
//	  	    }
//	  	});
