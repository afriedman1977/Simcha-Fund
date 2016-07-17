$(function () {
    $(".check").bootstrapSwitch();
    $("#search").on('input', function(){
        $("#contributors tr:gt(0)").each(function () {
            $(this).find("td:eq(1)").text().toLowerCase().search($("#search").val().toLowerCase()) < 0 && $("#search").val() != "" ? $(this).hide() : $(this).show();



            //var row = $(this);
            //var name = row.find("td:eq(1)").text().toLowerCase();            
            //var contains = $("#search").val().toLowerCase();            
            //var yes = name.search(contains);            
            //if (yes <= 0 && contains != "") {
            //    row.hide();
            //} else {
            //    row.show();
            //}
            
           // var name = $(this).find("td:eq(1)").text().toLowerCase();
            //var yes = $(this).find("td:eq(1)").text().toLowerCase().search($("#search").val().toLowerCase())
            //yes < 0 && $("#search").val() != "" ? $(this).hide() : $(this).show();            
        });
    });

    $("#clear").click(function () {
        $("#contributors tr:gt(0)").each(function () {
            $(this).show();
        })       
        $("#search").val("");
    })   
});

