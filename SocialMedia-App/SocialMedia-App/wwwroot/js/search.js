$(document).ready(function () {
    var searchInput = $('#search-input');
    var searchResults = $('#search-results');
    var dropdown = searchInput.length ? new bootstrap.Dropdown(searchInput[0]) : null;
    var typingTimer;
    var doneTypingInterval = 300;

    if (!searchInput.length || !searchResults.length) {
        return;
    }

    searchInput.on('input', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(performSearch, doneTypingInterval);
    });

    function performSearch() {
        var query = searchInput.val();

        if (query.length < 2) {
            searchResults.html('<li><span class="dropdown-item-text">Start typing to search...</span></li>');
            hideDropdown();
            return;
        }

        $.ajax({
            url: '/User/Home/Search',
            method: 'GET',
            data: { query: query },
            success: function (data) {
                searchResults.empty();

                if (data.length > 0) {
                    data.forEach(function (result) {
                        if (!result.url || !result.title) {
                            console.error('Search result missing required fields:', result);
                            return;
                        }

                        searchResults.append(createSearchResultItem(result));
                    });
                } else {
                    searchResults.html('<li><span class="dropdown-item-text">No results found</span></li>');
                }

                showDropdown();
            },
            error: function (xhr) {
                console.error('Search request failed:', xhr.responseText);
                searchResults.html('<li><span class="dropdown-item-text">Error occurred while searching</span></li>');
                showDropdown();
            }
        });
    }

    function createSearchResultItem(result) {
        var link = $('<a>')
            .addClass('dropdown-item border-bottom py-2')
            .attr('href', result.url)
            .css({
                'display': 'flex',
                'align-items': 'center',
                'gap': '10px'
            })
            .on('click', function () {
                searchInput.val(result.title);
                hideDropdown();
            });

        if (result.imageUrl) {
            link.append(
                $('<img>').attr('src', result.imageUrl).addClass('profile-img').css({
                    'width': '34px',
                    'height': '34px',
                    'border-radius': '50%',
                    'object-fit': 'cover',
                    'flex-shrink': '0'
                })
            );
        }

        var content = $('<div>').addClass('d-flex flex-column text-start');
        var resultType = result.type === 'post' ? 'Post' : 'User';

        content.append(
            $('<span>').addClass('fw-semibold').text(result.title)
        );

        if (result.subtitle) {
            content.append(
                $('<small>').addClass('text-muted').text(resultType + ' - ' + result.subtitle)
            );
        }

        link.append(content);

        return $('<li>').append(link);
    }

    function showDropdown() {
        if (dropdown) {
            dropdown.show();
        }
    }

    function hideDropdown() {
        if (dropdown) {
            dropdown.hide();
        }
    }

    searchInput.on('focus', function () {
        if (searchInput.val().length >= 2) {
            showDropdown();
        }
    });

    $(document).on('click', function (e) {
        if (!$(e.target).closest('.dropdown').length) {
            hideDropdown();
        }
    });
});
