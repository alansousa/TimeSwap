$(document).ready(function () {
    /*$.urlParam = function (name) {
        var results = new RegExp('[\?&amp;]' + name + '=([^&amp;#]*)').exec(window.location.href);
        return results[1] || 0;
    }*/

    $(".btnNovaHabilidade").click(function () {
        $("#habilidade").append("<div class='novaHabilidade'>" + $(".criarnovaHabilidade").html() + "");
    });
    
    $(document).on("click", ".btnApagarHabilidade", function () {

        var nivel = $(this).parent().find('.nivel').val(),
            id = $('#MATRICULA').val(),
            habilidade = $(this).parent().find('.listhab').val();

        var sucesso = false;

        $.ajax({
            type: 'POST',
            url: '/Recursos/ApagarHabilidade/',
            data: {idRecurso: id, idHabilidade: habilidade },
            success: function (result) {
                if (result != "")
                    sucesso = true;
            }
        }).done(function () {
            if (sucesso) {
                $(this).parent().remove();
                $.bootstrapGrowl("Vinculo excluído!", {
                    type: 'success',
                    align: 'center',
                    width: 'auto'
                });
            } else {
                $.bootstrapGrowl("Ops, não salvou!", {
                    type: 'danger',
                    align: 'center',
                    width: 'auto'
                });
            }
        });

    });

    $(document).on("click", ".btnSalvarHabilidade", function () {
        var nivel = $(this).parent().find('.nivel').val(),
            id = $('#MATRICULA').val(),
            habilidade = $(this).parent().find('.listhab').val();
        var sucesso = false;

        $.ajax({
            type: 'POST',
            url: '/Recursos/NovaHabilidade/',
            data: { nivel:nivel, idRecurso:id ,idHabilidade:habilidade},
            success: function (result) {
                if (result !="")
                    sucesso = true;
            }
        }).done(function () {
            if (sucesso) {
                $.bootstrapGrowl("Salvo!", {
                    type: 'success',
                    align: 'center',
                    width: 'auto'
                });
            } else {
                $.bootstrapGrowl("Ops, não salvou!", {
                    type: 'danger',
                    align: 'center',
                    width: 'auto'
                });
            }
        });
    });
});