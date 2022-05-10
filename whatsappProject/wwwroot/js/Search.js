$(function () {
    $('#queryForm').submit(e => {
        e.preventDefault();
        const q = $('#search').val();

        $('tbody').load('/FeedBacks/search?query='+q);
    })
});