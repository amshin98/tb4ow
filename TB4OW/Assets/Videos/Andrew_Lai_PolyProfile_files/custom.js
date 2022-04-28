$(document).ready(function() {
 	//Green bar - zebra striping
 	$("tr:even").addClass("even");
 	$("tr:odd").addClass("odd");
 	

 	// Hide the search form by default for all pages except the starting search page
	if ($('div#searchBoxWrapper').closest('html').length) {

		//Append a different title for the search page
		$('div#searchBoxWrapper').prepend('<h3>Search</h3>');

		//Show searchForm on default search page
		$('#searchForm').show();

		// remove the newsearch links on the primary search page
		$('.newSearch').parent('li').remove();
		$('.newSearch').remove();

		// Put the cursor into the first form element
		$("#searchForm :input:visible:enabled:first").focus();
	} else {

		// hide the search form for all other pages (also being done in css - not accessible)
		$('#searchForm').hide();

		// Make the New Search Form showable/hideable for all pages except the starting search page
		$('.newSearch').click(function(){

			$('#searchForm').slideToggle('fast');

			// Put the cursor into the first form element
			$("#searchForm :input:visible:enabled:first").focus();
		});
	}



	//Search Form selective input clearing
	$('#searchForm input[type="text"]').blur(function(){		
		//console.log('search form inputs have the focus');
		if($(this).attr('name') == 'p_first_name' || $(this).attr('name') == 'p_last_name') {
			//console.log('First and Last Name');
			if($(this).val() != '') {
				$('#searchForm input[name="p_organization"]').val('');
				$('#searchForm input[name="p_id_number"]').val('');
			}
		}

		if($(this).attr('name') == 'p_organization'  ) {
			//console.log('Organization');
			if($(this).val() != '') {
				$('#searchForm input[name="p_first_name"]').val('');
				$('#searchForm input[name="p_last_name"]').val('');
				$('#searchForm input[name="p_id_number"]').val('');
			}
		}

		if($(this).attr('name') == 'p_id_number'  ) {
			//console.log('ID Number');
			if($(this).val() != '') {
				$('#searchForm input[name="p_first_name"]').val('');
				$('#searchForm input[name="p_last_name"]').val('');
				$('#searchForm input[name="p_organization"]').val('');
			}
		}
	});	


	// Add .scroll class to the jump nav
	$('ul#innerSubNav_sections').children().addClass('scroll');

	// Smooth Scroll the jump links
	$(".scroll a").click(function(event){		
		event.preventDefault();
		$('html,body').animate({scrollTop:$(this.hash).offset().top}, 200);
	});



	// Add iconography to Collapsible / Expandable Sections
	$(".sectionTitle").each(function(){
		// Check if the section has results
		var noResults = $(this).next('.sectionContent').children('div.no-results').length;
		if (!noResults) {
			// Check if the section is currently visible
			if ($(this).next('.sectionContent').is(":visible")   ) {
				// set a collapse icon
				$(this).next(".sectionContent").prev(".sectionTitle").prepend('<i class="icon-collapse-alt"></i> ');
			} else {
				// set an expand icon
				$(this).next(".sectionContent").prev(".sectionTitle").prepend('<i class="icon-expand-alt"></i> ');
			}
		}
	});


	// Make all sections showable/hideable
	$('.sectionTitle').click(function(){

		var noResults = $(this).next('.sectionContent').children('div.no-results').length;

		if (!noResults) {
			if ( $(this).next('.sectionContent').is(":visible") ) {
				$(this).next('.sectionContent').slideUp('fast');
				$(this).children("i").removeClass("icon-collapse-alt").addClass("icon-expand-alt");
			} else {
				$(this).next('.sectionContent').slideDown('fast');
				$(this).children("i").removeClass("icon-expand-alt").addClass("icon-collapse-alt");
			}
		}

	});






	// Allow for collapsed content by default
	$(".collapsed").each(function(){
		$(this).hide();
	});



	// Add iconography to Collapsible / Expandable Content
	$(".expandTrigger").each(function(){
		
		//Check if the expandContent has anything within it
		var siblingContent = $(this).next('.expandContent').text().trim().length;
		var childContent = $(this).children('.expandContent').text().trim().length;



		//var hasContent = $(this).next('.expandContent').text().length;
		var elementName = $(this).prop("tagName");



		// Things we don't want icons to show up for
		if (elementName == 'TABLE' || elementName == 'TR' || elementName == 'TD') {
			var showIcons = false;
		} else {
			var showIcons = true;
		}


		/*console.log('Element: ' + elementName);
		console.log('Sibling: ' + siblingContent);
		console.log('Child: ' + childContent);
		console.log('ShowIcons: ' + showIcons);*/

		if (showIcons && siblingContent) {
			// Check if the expandContent is currently visible
			if ($(this).next('.expandContent').is(":visible")   ) {
				// set a collapse icon
				$(this).next(".expandContent").prev(".expandTrigger").prepend('<i class="icon-collapse-alt"></i> ');
			} else {
				// set an expand icon
				$(this).next(".expandContent").prev(".expandTrigger").prepend('<i class="icon-expand-alt"></i> ');
			}
		}

		if (showIcons && childContent) {
			// Check if the expandContent is currently visible
			if ($(this).children('.expandContent').is(":visible")   ) {
				// set a collapse icon
				$(this).children(".expandContent").parent(".expandTrigger").prepend('<i class="icon-collapse-alt"></i> ');
			} else {
				// set an expand icon
				$(this).children(".expandContent").parent(".expandTrigger").prepend('<i class="icon-expand-alt"></i> ');
			}
		}
	});


	// Make Poly Plans and other stuff showable/hideable
	$('.expandTrigger').click(function(){

		//Check if the expandContent has anything within it
		var siblingContent = $(this).next('.expandContent').text().trim().length;
		var childContent = $(this).children('.expandContent').text().trim().length;

		if (siblingContent) {			
			if ( $(this).next('.expandContent').is(":visible") ) {
				$(this).next('.expandContent').fadeOut('slow').slideUp('fast');
				$(this).children("i").removeClass("icon-collapse-alt").addClass("icon-expand-alt");
			} else {
				$(this).next('.expandContent').fadeIn('slow').slideDown('fast');
				$(this).children("i").removeClass("icon-expand-alt").addClass("icon-collapse-alt");
			}
		}

		if (childContent) {
			if ( $(this).children('.expandContent').is(":visible") ) {
				$(this).children('.expandContent').fadeOut('slow').slideUp('fast');
				$(this).children("i").removeClass("icon-collapse-alt").addClass("icon-expand-alt");
			} else {
				$(this).children('.expandContent').fadeIn('slow').slideDown('fast');
				$(this).children("i").removeClass("icon-expand-alt").addClass("icon-collapse-alt");
			}
		}

	});












	// append/insert the print page link via javascript (depending if we have a logout link)
	var printLink = function(){
		var link = '<li class="print"><a href="#print">Print Page</a></li>';
		var nav = $('ul#innerNav_menu');
		var logout = $('ul#innerNav_menu li.logout');



		if (logout.length > 0) {
			logout.before(link);
		} else {	
			nav.append(link);
		}
	};

	// execute the printLink function
	printLink();

	//$('ul#innerNav_menu').append('<li class="print"><a href="#print">Print Page</a></li>');

	// bind a click event to the printLink
	$('ul#innerNav_menu li.print a').click(function(event) {
		window.print();
		event.preventDefault();
	});


	// define the floating navigation function
    var floating_navigation = function(){

    	var nav = $('#contentNav');
		var nav_adjust = 0;
		var nav_position = 'nuttin honey';
		var nav_height = nav.outerHeight();

		var nav_minHeight = $('#contentLine').offset().top + $('#contentLine').outerHeight(true);
		var nav_maxHeight = $('#mainLeftFull').offset().top + $('#mainLeftFull').outerHeight();
		var nav_move = false;
			
		// current vertical position from the top
        var scroll_top = $(window).scrollTop(); 	

        //check to make sure we have the nav visible on the page
		if (nav.closest('html').length) {

			//Get the current nav position
			nav_position = nav.offset().top;

			// figure out if we are scrolling up or down... and adjust the nav
			// Scrolling Down
			if (scroll_top > nav_position && nav_position < (nav_maxHeight - nav_height)) { 
				
				nav_adjust = scroll_top - nav_minHeight;

				nav_move = true;
				//console.log('scrolling down >>>>>>>>');
			}
			// Scrolling Up
			else if (scroll_top < nav_position && nav_position > nav_minHeight) {

				nav_adjust = scroll_top - nav_minHeight;

				// check to keep nav from resetting to absolute top (page top - negative margin)
				if (nav_adjust < 0) {
					nav_adjust = 0;
					//console.log('setting nav_adjust to: ' + nav_adjust);
				}
				nav_move = true;
				//console.log('<<<<<<<<< scrolling up');
			}	

			if (nav_move) {
				nav.animate({
					'margin-top': nav_adjust
				},
				{ 
					duration: '20',
					easing: 'linear',
					queue: false 
				});
			}
		}
        /*
		console.log('nav position:' + nav_position);
		console.log('nav height:' + nav_height);
        console.log('scroll:' + scroll_top);
        console.log('nav adjust:' + nav_adjust);
        console.log('minHeight:' + nav_minHeight);
        console.log('maxHeight:' + nav_maxHeight);
        */
    };
     
    // run function on load
    floating_navigation();
     
    // run function every time you scroll
    $(window).scroll(function() {
         floating_navigation();
    });


    // Trigger the date picker for any date form fields
    $(".datePicker").datepicker();
});