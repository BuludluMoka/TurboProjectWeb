var api = null;
$.ajaxSetup({
    headers: { 'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val() },
    error: function (xhr, status, error) {
        console.log(error);
    }
});
$(document).ready(function () {
    var CheckVinUrl = $('#reportUrl').val();
    //alert(CheckVinUrl);
    //$.cookieBar({
    //    fixed: true,
    //    message: 'Biz istifadə və seçimləri izləmək üçün kuki faylları istifadə edirik',
    //    acceptText: 'Mən qəbul edirəm',

    //    policyText: 'Məxfilik siyasəti',
    //    policyURL: 'https://vini.az/az/mexfilik-siyaseti',
    //});
    // API
    api =
    {
        CheckOneReport: function (vin, type) {
            swal.fire({
                title: 'VİN ilə axtarış',
                html: 'Axtarış bir neçə dəqiqə çəkə bilər...',
                allowOutsideClick: false,
                allowEscapeKey: false,
                didOpen: function () {
                    swal.showLoading();
                    $.get(CheckVinUrl, { vin: vin, type: type }, function (resp) {
                        if (resp.status == "failed") {
                            return api.showError(resp.message);
                        }
                        swal.fire({
                            icon: 'question',
                            title: 'Məlumat mövcuddur',
                            html: 'Ödənişə keçmək istəyirsinizmi?',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            showConfirmButton: true,
                            confirmButtonText: 'Ödənişə keçmək!',
                            confirmButtonColor: '#8ec549',
                            cancelButtonColor: '#eb525b',
                            showCancelButton: true,
                            cancelButtonText: 'Xeyr',
                        }).then(function (result) {
                            if (result.value) {
                                swal.showLoading();
                                window.location = resp.url;
                            }
                        })
                    })
                }
            });
        },
        CheckAllReport: function (vin) {
            swal.fire({
                title: 'VİN ilə axtarış',
                html: 'Axtarış bir neçə dəqiqə çəkə bilər...',
                allowOutsideClick: false,
                allowEscapeKey: false,
                didOpen: function () {
                    swal.showLoading();
                    $.ajax({
                        type: "GET",
                        url: "/Administration/Index?handler=CheckAllReports",
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        data: { "vin": vin },
                        success: function (response) {
                            $("#report-result").html(response);
                            swal.close();
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });


                   
                }
            });
        },
        showError: function (error) {
            swal.hideLoading()

            swal.fire({
                icon: 'error',
                title: 'An error occurred...',
                html: error,
                confirmButtonColor: '#d33',
                footer: '<a href="/Home/FAQ">Niyə bu xetanı alıram?</a>'
            })
        },
        showSuccess: function (message) {
            swal.fire({
                type: 'success',
                title: 'Hər şey əladır!',
                html: message.text,
                allowOutsideClick: false,
                allowEscapeKey: false,
                onOpen: function () {
                    swal.showLoading();

                    if (typeof message.url != 'undefined') {
                        setTimeout(function () {
                            location.href = message.url;
                        }, 2000);
                    }
                }
            })
        },
    }
    $('#check-report-form').submit(function (e) {
        e.preventDefault();
        var vin = $('input[name = "vin-check-input"]').val();
        var type = $('input[name = "reportType"]:checked').val();
        if (vin.length != 17) {
            return api.showError('Wrong VIN number or it is not registered in the US!');
        }
        if (type == undefined) {
            return api.showError('Please choose the type of the report');
        }
        api.CheckOneReport(vin,type);
    });
    $('#check-administration-report').submit(function (e) {
        e.preventDefault();
        var vin = $('input[name = "vin-check-input"]').val();
        if (vin.length != 17) {
            return api.showError('Wrong VIN number or it is not registered in the US!');
        }
        api.CheckAllReport(vin);
    });
    $('#check-report-bottom-form').submit(function (e)   {
        e.preventDefault();

        var vin = $('input[name = "vin-check-bottom-input"]').val();
        var type = $('input[name = "reportTypeBottom"]:checked').val();

        if (vin.length != 17) {
            return api.showError('Wrong VIN number or it is not registered in the US!');
        }
        if (type == undefined) {
            return api.showError('Please choose the type of the report');
        }
        api.CheckOneReport(vin,type);
    })
});









//var api = null;

//$.ajaxSetup({
//	headers: { 'X-CSRF-TOKEN': 'fjYIxndqtf9ISLOBG2hKF6dxp4kHS3FlPju3ZT7e' },
//	error: function (xhr, status, error) {
//		console.log(error);
//		api.retryLookup();
//	}
//});

//$(document).ready(function () {

//	$.cookieBar({
//		fixed: true,
//		message: 'Biz istifadə və seçimləri izləmək üçün kuki faylları istifadə edirik',
//		acceptText: 'Mən qəbul edirəm',

//		policyText: 'Məxfilik siyasəti',
//		policyURL: 'https://vini.az/az/mexfilik-siyaseti',
//	});

//	if ($('.static-page').length) {
//		$('ul,ol', $('.static-page')).addClass('bullets');
//	}


//	// LOOKUP PLACEHOLDER
//	$('.lookup-type').click(function () {
//		var searchInput = $('.lookup-vin');
//		var stdPlaceholder = "VIN(BAN) kodu daxil edin və Axtarış düyməsini basın...";
//		var jpPlaceholder = "Şassi kodu daxil edin və Axtarış düyməsini basın...";

//		if ($(this).val() == 'jp') {
//			searchInput.attr('placeholder', jpPlaceholder);
//		} else {
//			searchInput.attr('placeholder', stdPlaceholder);
//		}
//	})

//	// API
//	api =
//	{
//		lastVin: null,
//		lastType: null,
//		isLookup: false,
//		retryNumber: 0,

//		resetRetries: function () {
//			this.lastVin = null;
//			this.lastType = null;
//			this.isLookup = false,
//				this.retryNumber = 0;
//		},

//		retryLookup: function () {
//			if (this.isLookup) {
//				if (this.retryNumber == 2) {
//					this.showError('Səhv baş verdi. Zəhmət olmasa bir az sonra yenə cəhd edin.');

//					return this.resetRetries();
//				}

//				this.retryNumber++;
//				this.lookup(this.lastVin, this.lastType);
//				ym(51480544, 'reachGoal', 'error_retry');
//			}
//		},

//		lookup: function (vin, type) {
//			this.lastVin = vin;
//			this.lastType = type;
//			this.isLookup = true;

//			fbq('track', 'Search');

//			swal({
//				title: 'VİN ilə axtarış',
//				html: 'Axtarış bir neçə dəqiqə çəkə bilər...',
//				allowOutsideClick: false,
//				allowEscapeKey: false,
//				onOpen: function () {
//					swal.showLoading();

//					$.post('https://vini.az/az/lookup', { vin: vin, type: type }, function (resp) {
//						if (!resp.status) {
//							return api.showError(resp.errors);
//						}

//						if (resp.message.type == 'offer') {
//							return api.offerBuy(resp.message.url);
//						}

//						if (resp.message.type == 'specify') {
//							return api.specifyCar(resp.message);
//						}

//						api.showSuccess(resp.message);
//					})
//				},
//				onClose: function () {

//				}
//			});
//		},

//		showError: function (error) {
//			swal.hideLoading()
//			//swal.showValidationMessage('test message')

//			swal({
//				type: 'error',
//				title: 'Səhv baş verdi!',
//				html: error,
//			})
//		},

//		showSuccess: function (message) {
//			swal({
//				type: 'success',
//				title: 'Hər şey əladır!',
//				html: message.text,
//				allowOutsideClick: false,
//				allowEscapeKey: false,
//				onOpen: function () {
//					swal.showLoading();

//					if (typeof message.url != 'undefined') {
//						setTimeout(function () {
//							location.href = message.url;
//						}, 2000);
//					}
//				}
//			})
//		},

//		offerBuy: function (url) {
//			swal({
//				type: 'question',
//				// title: 'Hər şey əladır!',
//				title: 'Məlumat mövcuddur!',
//				html: 'Hesabat mövcuddur, ödənişə keçmək istəyirsinizmi?',
//				allowOutsideClick: false,
//				allowEscapeKey: false,
//				showConfirmButton: true,
//				confirmButtonText: 'Ödənişə keçmək!',
//				confirmButtonColor: '#8ec549',
//				cancelButtonColor: '#eb525b',
//				showCancelButton: true,
//				cancelButtonText: 'Heyr...',
//			}).then(function (result) {
//				if (result.value) {
//					swal.showLoading();
//					location.href = url;
//				}
//				else if (result.dismiss === Swal.DismissReason.cancel) {
//					ym(51480544, 'reachGoal', 'cancel_buy');
//				}
//			})
//		},

//		checkout: function (form) {
//			var button = $('.checkout-button');

//			button
//				.after('<i class="icon-spin4 animate-spin loader"></i>')
//				.attr('disabled', 'disabled');

//			$.post('https://vini.az/az/checkout', form.serialize(), function (resp) {
//				if (!resp.status) {
//					$('.loader', form).remove();
//					button.removeAttr('disabled');

//					if (typeof resp.errors.gateway != 'undefined') {
//						return api.showError(resp.errors.gateway);
//					}

//					$.each(resp.errors, function (k, e) {
//						$(':input[name="' + k + '"]').addClass('error');
//					})

//					$('body,html').animate({
//						scrollTop: 0
//					}, 500);

//					return;
//				}

//				location.href = resp.message.url;
//			})
//		},



//		createReport: function () {
//			var loaderButton = $('#loader-button');
//			var viewButton = $('#view-button');
//			var waitBlock = $('#wait-block');

//			$.getJSON(loaderButton.data('url'), function (resp) {
//				if (!resp.status) {
//					api.showError(resp.errors)

//					return setTimeout('location.reload();', 10000);
//				}

//				loaderButton.addClass('hidden');

//				if (typeof resp.message.type != 'undefined' && resp.message.type == 'wait') {
//					return waitBlock.removeClass('hidden');
//				}

//				viewButton.removeClass('hidden');
//			})
//		},

//		specifyCar: function (search) {
//			var carsHtml = $('#carsHtml');
//			var tmpl = carsHtml.find('.tmpl');
//			var clone = tmpl.clone();

//			tmpl.remove();

//			if (search.data.length > 0) {
//				var rowClass = search.data.length > 1 ? 'col-6' : 'col-12';

//				$.each(search.data, function (i, car) {
//					// DATA & CLASS
//					clone.find('.car-select')
//						.addClass(rowClass)
//						.attr('data-search_id', search.search_id)
//						.attr('data-car_id', car.car_id)

//					// IMAGE
//					clone.find('img').attr('src', 'https://carvx.jp' + car.image)

//					$.each(car, function (k, v) {
//						clone.find('#' + k).text(v);
//					});

//					carsHtml.append(clone);
//				});

//				swal({
//					//type: 'question',
//					//title: 'api.specify-car',
//					html: carsHtml,
//					allowOutsideClick: false,
//					allowEscapeKey: false,
//					showConfirmButton: true,
//					confirmButtonText: 'İrəli',
//					confirmButtonColor: '#8ec549',
//					cancelButtonColor: '#eb525b',
//					showCancelButton: true,
//					cancelButtonText: 'İmtina',
//					preConfirm: function () {
//						var selected = $('.car-select.selected');

//						if (!selected.length) {
//							alert('Avtomobilinizi aşağıdan seçin və "İrəli" tıklayın.');
//							return false;
//						}
//					}
//				}).then(function (result) {
//					if (result.value) {
//						var selected = $('.car-select.selected');

//						return api.prepareJpReport(selected.data('search_id'), selected.data('car_id'));
//					}
//					else if (result.dismiss === Swal.DismissReason.cancel) {
//						ym(51480544, 'reachGoal', 'cancel_buy');
//					}
//				})
//			}
//		},

//		prepareJpReport: function (search_id, car_id) {
//			swal({
//				title: 'Axtarış',
//				text: 'Yaponiya verilənlər bazalarında axtarış ...',
//				allowOutsideClick: false,
//				allowEscapeKey: false,
//				onOpen: function () {
//					swal.showLoading();

//					var post = {
//						'search_id': search_id,
//						'car_id': car_id,
//						'vin': this.lastVin
//					};

//					$.post('https://vini.az/az/carvx/prepare', post, function (resp) {
//						if (resp.status) {
//							return api.offerBuy(resp.message.url);
//						}

//						api.showError(resp.error)
//					})
//				}
//			})
//		},

//		/* API UI */
//		getApiData: function () {
//			var data = localStorage.getItem('api');

//			if (data == null) {
//				return false;
//			}

//			return JSON.parse(data);
//		},


//		logout: function () {
//			swal.showLoading();

//			localStorage.removeItem('api');

//			location.href = 'https://vini.az/az/ui/login';
//		},


//		getMe: function (apiKey, callback) {
//			$.get('https://vini.az/az/api/me?key=' + apiKey, function (resp) {
//				if (resp.status) {
//					api.setMe(resp)

//					if (callback) {
//						callback(resp)
//					}
//				}
//			})
//		},


//		setMe: function (data) {
//			localStorage.setItem('api', JSON.stringify(data));
//		},


//		login: function (form) {
//			swal.showLoading();

//			$.get(form.attr('action'), form.serialize(), function (resp) {
//				if (!resp.status) {
//					api.showError('Daxil edilmiş API açarı yanlışdır')

//					return;
//				}

//				api.setMe(resp)

//				location.href = 'https://vini.az/az/ui';
//			})
//		},


//		register: function (form) {
//			swal.showLoading();

//			$.get(form.attr('action'), form.serialize(), function (resp) {
//				swal.hideLoading();

//				if (!resp.status) {
//					$.each(JSON.parse(resp.error), function (k, e) {
//						return api.showError(e.join("\n"))
//					})

//					return;
//				}

//				swal({
//					type: 'info',
//					title: 'Hər şey əladır!',
//					html: 'API hesabı uğurla qeydiyyatdan keçdi<br />' +
//						'API açarı: ' + resp.key + '<br />' +
//						'Lütfən, API açarınızı təhlükəsiz yerə yazın.',
//					allowOutsideClick: false,
//					allowEscapeKey: false,
//					showConfirmButton: true,
//					confirmButtonText: 'Daxil ol',
//					confirmButtonColor: '#8ec549',
//					cancelButtonColor: '#eb525b',
//					showCancelButton: true,
//					cancelButtonText: 'İmtina',
//				}).then(function (result) {
//					if (result.value) {
//						swal.showLoading();
//						location.href = 'https://vini.az/az/ui/login';
//					}
//				})
//			})
//		},

//		replenish: function (onPaymentCloseCallback) {
//			var apiData = api.getApiData();

//			if (apiData == null) {
//				location.href = 'https://vini.az/az/ui/login';
//				return;
//			}

//			swal({
//				title: 'Balansı artırmaq',
//				html: 'Məbləğ: <input type="number" min="' + apiData.price + '" value="' + apiData.price + '" step="' + apiData.price + '" id="amount" class="form-control" style="width:100px; display: inline-block; margin:20px 0;" required /> AZN ($<span id="amount_usd">0</span>)<br /><small>Uğurlu ödənişdən sonra sadəcə açılmış pəncərəni bağlayın</small>',
//				allowOutsideClick: false,
//				allowEscapeKey: false,
//				showConfirmButton: true,
//				confirmButtonText: 'Ödəmək',
//				confirmButtonColor: '#8ec549',
//				cancelButtonColor: '#eb525b',
//				showCancelButton: true,
//				cancelButtonText: 'İmtina',
//				onOpen: function () {
//					$('#amount').change()
//				}
//			}).then(function (result) {
//				if (result.value) {
//					swal.showLoading();

//					var payment = window.open('https://vini.az/az/api/replenish?key=' + apiData.key + '&amount=' + $('#amount').val());

//					var pollTimer = window.setInterval(function () {
//						if (payment.closed !== false) { // !== is required for compatibility with Opera
//							window.clearInterval(pollTimer);

//							if (onPaymentCloseCallback) {
//								onPaymentCloseCallback()
//							}
//						}
//					}, 200);
//				}
//			})
//		},


//		checkVin: function (button) {
//			var form = button.parents('form')
//			form.attr('action', button.attr('href'))

//			swal({
//				title: 'VİN ilə axtarış',
//				html: 'Axtarış bir neçə dəqiqə çəkə bilər...',
//				allowOutsideClick: false,
//				allowEscapeKey: false,
//				onOpen: function () {
//					swal.showLoading();

//					$.get(form.attr('action'), form.serialize(), function (resp) {
//						if (!resp.status) {
//							api.showError(resp.error)

//							return;
//						}

//						swal({
//							type: 'question',
//							title: 'Məlumat mövcuddur!',
//							html: 'Hesabat mövcuddur, ödənişə keçmək istəyirsinizmi?',
//							allowOutsideClick: false,
//							allowEscapeKey: false,
//							showConfirmButton: true,
//							confirmButtonText: 'Hesabatı almaq',
//							confirmButtonColor: '#8ec549',
//							cancelButtonColor: '#eb525b',
//							showCancelButton: true,
//							cancelButtonText: 'Heyr...',
//						}).then(function (result) {
//							if (result.value) {
//								$('#api_report').click()
//							}
//						})
//					})
//				}
//			})
//		},


//		buyReport: function (button) {
//			var form = button.parents('form')
//			form.attr('action', button.attr('href'))
//			form.submit();
//		},

//	}

//	//api.specifyCar({"type":"specify","search_id":"X7mnMC5MK2FP","data":[{"car_id":0,"chassis_number":"NZE121-3065178","make":"TOYOTA","model":"COROLLA SPACIO","grade":"X","manufacture_date":"2001-06","body":"DBA-NZE121","engine":"1NZFE","drive":"FF(front engine, front-wheel drive)","transmission":"AT","image":"\/search\/img\/catalog\/10101053_200412.jpg"}]});

//	$('#lookup-form').submit(function (e) {
//		e.preventDefault();

//		var vin = $('.lookup-vin').val();
//		var type = $('.lookup-type:checked').val();

//		/*if (vin.length != 17){
//			return api.showError('Səhv VİN (BAN) nömrə və ya o ABŞ-da qeyd olunmayıb!');
//		}*/

//		api.lookup(vin, type);
//	})


//	$('.buy-button').click(function (e) {
//		e.preventDefault();

//		var vin = $(this).data('vin');

//		api.lookup(vin, 'vini');
//		//api.lookup(vin, 'carfax');
//	})


//	$('.checkout-form').submit(function (e) {
//		e.preventDefault();

//		api.checkout($(this));
//	})


//	$('.print-button').click(function (e) {
//		e.preventDefault();

//		window.print();
//	})

//	$('.check-info').click(function () {
//		$(this).siblings('.hidden').toggle();
//	})

//	$(document).on('click', '.car-select', function () {
//		$(this).siblings().removeClass('selected')
//		$(this).toggleClass('selected')
//	})
//});
