$(document).ready(function () {
    var searchInput = $('#search-input');
    var searchResults = $('#search-results');
    var dropdownMenu = $('#search-dropdown');
    var dropdown = new bootstrap.Dropdown(searchInput[0]);
    var typingTimer;
    var doneTypingInterval = 300;

    searchInput.on('input', function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(performSearch, doneTypingInterval);
    });

    function performSearch() {
        var query = searchInput.val();
        if (query.length < 2) {
            searchResults.html('<li><span class="dropdown-item-text">Start typing to search...</span></li>');
            dropdown.hide();
            return;
        }

        $.ajax({
            url: '/User/Home/Search',  // Corrected URL to point to the HomeController inside the User area
            method: 'GET',
            data: { query: query },
            success: function (data) {
                searchResults.empty();
                if (data.length > 0) {
                    data.forEach(function (user) {
                        if (user.id && user.userName) {
                            searchResults.append(
                                $('<li>').append(
                                    $('<a>').addClass('dropdown-item border')
                                    .css({
                                        'display': 'flex',
                                        'align-items': 'center' 
                                    })
                                    .append(
                                        $('<img>').attr('src', user.profileImageURL).addClass('profile-img').css({
                                            'width': '30px', 
                                            'height': '30px',
                                            'border-radius': '50%', 
                                            'margin-right': '10px' 
                                        })
                                    )
                                    .append(
                                        $('<span>').text(user.userName)
                                    )
                                    .attr('href', '#')
                                    .data('userid', user.id)
                                    .click(function (e) {
                                        e.preventDefault();
                                        var userId = $(this).data('userid');
                                        searchInput.val(user.userName);
                                        dropdown.hide();    
                                        sendUserRequest(userId);
                                    })
                                )
                            );
                        } else {
                            console.error('User object missing id or username:', user);
                        }
                    });
                } else {
                    searchResults.html('<li><span class="dropdown-item-text">No results found</span></li>');
                }
                dropdown.show();
            },
            error: function (xhr, status, error) {
                console.error("Search request failed:", xhr.responseText);
                searchResults.html('<li><span class="dropdown-item-text">Error occurred while searching</span></li>');
                dropdown.show();
            }
        });

    }

    function sendUserRequest(userId) {
        if (userId) {
            // Redirecting to the profile page using the `userId`
            window.location.href = '/User/Profile?userId=' + userId;
        } else {
            console.error("Invalid userId passed to sendUserRequest:", userId);
        }
    }

    // Show dropdown when focusing on input
    searchInput.on('focus', function () {
        if (searchInput.val().length >= 2) {
            dropdown.show();
        }
    });

    // Hide dropdown when clicking outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('.dropdown').length) {
            dropdown.hide();
        }
    });
});
