

function LoadingAc() {
    $("#preloader").show();
    $('#wrapper').addClass("spinner-blur");
}

function LoadingKapat() {
    $("#preloader").fadeOut();
    $("#wrapper").removeClass("spinner-blur");
}

function getContents(controller, action, param) {
    var link = "/" + controller + "/" + action;
    if (param != "" && param != "0") {
        link += "/" + param;
    }
    //$.ajax({
    //    url: "/" + controller + "/OlcuBirimleri",
    //    type: 'post',
    //    dataType: 'html',
    //    async: true,
    //    success: function (data) {
    //        $("#mainContent").html('<div style="display:none;">' + data + '</div>');
    //    },
    //    error: function (s, e) {
    //        HataGetir("Sayfa yüklenemedi, lütfen tekrar deneyiniz.");
    //    }
    //});

    $.ajax({
        url: link,
        type: 'post',
        dataType: 'html',
        async: true,
        success: function (data) {
            $("#preloader").show();
            $("#mainContent").html(data);
            $("#preloader").fadeOut();
        },
        error: function (s, e) {
            HataGetir("Page couldn't load, please try again.");
        }
    });

}

function HataGetir(mesaj) {
    if (mesaj != '') {
        $("#uyariModal .modal-body").html(mesaj);
    }
    $("#uyariModal").modal();
}

function ModalAc(modal) {

    $('#modal' + modal).modal({
        backdrop: 'static',
        keyboard: false,
        show: true
    });
}

function ModalKapat(modal) {
    $('#modal' + modal).modal('hide');
    $("#div" + modal + "Uyari").css('display', 'none');
    $(".modal-backdrop").remove();
}

function RemoveRecord(url, page) {

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: url,
        dataType: "json",
        async: false,
        success: function (yanit) {
            if (yanit.status != "1") {
                $('#divKayitSilUyari').html(yanit.statusText.toString());
                $('#divKayitSilUyari').show();
            } else {
                ModalKapat('KayitSil');
                getContents('Sayfalar', page);
            }
        },
        error: function (Result) {
            $('#divKayitSilUyari').html("Hata oluştu, lütfen tekrar deneyiniz.");
            $('#divKayitSilUyari').show();
        }
    });
}
