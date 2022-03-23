$(() => {

    $(".deposit-button").on('click', function () {

        const ContributorId = $(this).data('contribid')
        const fullName = $(this).data('fullname')
        
        $(".deposit").modal();
        $("#contrib-id").val(ContributorId);
        $("#deposit-name").text(fullName);
       
    })

    $("#new-contributor").on('click', function () {
        $(".new-contrib").modal();
    })

    $(".edit-contrib").on('click', function () {

        const firstName = $(this).data('first-name')
        
        const lastName = $(this).data('last-name')
        const cell = $(this).data('cell')
        const alwaysInclude = $(this).data('always-include')
        console.log(alwaysInclude)
        const date = $(this).data('date')
        const id = $(this).data('id')
        $(".new-contrib").modal();
        

        $(".contrib-form").attr('action', '/Contributors/UpdateContributor')
        $(".contrib-form").append(`<input type = "hidden" id = "edit-id" name = "id" value='${id}'/>`)

        $("#contributor_first_name").val(firstName);
        $("#contributor_last_name").val(lastName);
        $("#contributor_cell_number").val(cell);
        if (alwaysInclude === "True") {
            $("#contributor_always_include").prop('checked', true);
        }
        else {
            $("#contributor_always_include").prop('checked', false);
        }
        
        $("#contributor_created_at").val(date);
    
        $("#initialDepositDiv").remove()


    })


})