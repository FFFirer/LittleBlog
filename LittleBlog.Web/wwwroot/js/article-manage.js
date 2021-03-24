function deleteArticle(url, id) {
    $.ajax({
        url: url.replace("{id}", id),
        type: "post",
        success: function (json) {
            return json;
        },
        error
    })
}