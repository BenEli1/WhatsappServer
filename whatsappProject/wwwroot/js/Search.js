$(fucntion(){
    $("#queryForm").submit(e => {
        e.preventDefault();
    })
    const q = $('#search').val();

    $("#TheBody").load('/FeedBack/Search?query=' + q);




})