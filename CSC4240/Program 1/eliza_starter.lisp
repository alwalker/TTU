;;==========================================================================
;;
;; STARTER FILE FOR CSC 4240/5240 PROJECT #1: Eliza
;; Borrowed from http://ranger.uta.edu/~tiernan/completed/CSE4308/Lab1Eliza.htm
;;==========================================================================

;;----------------------------------------------------------------------------
;; eliza: top-level function which, when given a sentence (no
;; punctuation, please!), comes back with a response like you would.

( defun eliza ( sentence )
  ( respond ( change-pros sentence ) database ) )

;;----------------------------------------------------------------------------
;; change-pros: changes the pronouns of the sentence so that Eliza can
;; come back with the appropriately switched first and second person
;; references.

( defun change-pros ( sentence )
  ( cond 
    ( ( null sentence ) nil )
    ( ( equal ( car sentence ) 'you )
      ( cons 'I ( change-pros ( cdr sentence ) ) ) )
    ( ( equal ( car sentence ) 'I )
      ( cons 'you ( change-pros ( cdr sentence ) ) ) )

    ;; CHANGE THIS: add more cases here of pronouns or other words
    ;; that should flip in order for this to work well
    ( ( equal ( car sentence ) 'my )
      ( cons 'your ( change-pros ( cdr sentence ) ) ) )
    ( ( equal ( car sentence ) 'your )
      ( cons 'my ( change-pros ( cdr sentence ) ) ) )
      
    ( ( equal ( car sentence ) 'mine )
      ( cons 'yours ( change-pros ( cdr sentence ) ) ) )
    ( ( equal ( car sentence ) 'yours )
      ( cons 'mine ( change-pros ( cdr sentence ) ) ) )
      
    ( ( equal ( car sentence ) 'myself )
      ( cons 'yourself ( change-pros ( cdr sentence ) ) ) )
    ( ( equal ( car sentence ) 'yourself )
      ( cons 'myself ( change-pros ( cdr sentence ) ) ) ) 
      
    ( ( equal ( car sentence ) 'me )
      ( cons 'you ( change-pros ( cdr sentence ) ) ) ) 
    (t ( cons( car sentence ) ( change-pros ( cdr sentence ) ) ) ) ) )
    
    

;;----------------------------------------------------------------------------
;; respond: given a sentence, looks through the database in search of
;; a matching pattern and the response; given the database response,
;; uses 'instantiate' to fill in the blanks, and returns the completed
;; response

( defun respond ( sentence db )
  ( cond
    ;; end of DB, return nil - should never really happen
    ( ( null db ) (nth (random (length catch-alls) ) catch-alls ) ) 

    ;; if the result of matching the sentence against the current
    ;; pattern is a success, produce this response
    ( ( success ( setq result ( match sentence ( first ( car db ) ) ) ) )
      ( instantiate result ( second ( car db ) ) ) )

    ;; otherwise, keep looking through the DB
    ( t ( respond sentence ( cdr db ) ) ) ) )

;;----------------------------------------------------------------------------
;; match: if there is not a match between this pattern and this data,
;; returns 'fail;' otherwise, returns the sentence in partitioned
;; format

( defun match ( data pattern )
  ( cond
    ;; end of both data and pattern; a match
    ( ( and ( null data ) ( null pattern ) ) nil )

    ;; end of pattern, but not end of data; no match
    ( ( null pattern ) fail )

    ;; end of data, but not end of pattern; if the pattern starts with
    ;; a variable, eat it and try and match the rest of the pattern to
    ;; the null sentence (will only work if all variables); otherwise,
    ;; fail
    ( ( null data ) 
      ( cond
	( ( variablep ( car pattern ) )
	  ( if ( success ( setq result ( match data ( cdr pattern ) ) ) )
	      result
	    fail ) )
	( t fail ) ) )


    ;; first item of data and pattern are identical; if the rest of it
    ;; matched, return the first item cons'ed with the rest of the
    ;; partitioned sentence; otherwise, fail
    ( ( equal ( car data ) ( car pattern ) )
      ( if ( success ( setq result ( match ( cdr data ) ( cdr pattern ) ) ) )
	  ( cons ( list ( car data ) ) result )
	fail ) )

    ;; first item of pattern is a variable; if the rest of the data
    ;; (minus the first word, matched to the variable) is a match with
    ;; all of the pattern, return the appropriate stuff; if all of the
    ;; data (variable eats nothing) matches the rest of the pattern,
    ;; return appropriate stuff; else, fail.
    ( ( variablep ( car pattern ) ) 
      ( cond
	;; variable eats nothing;  () is put in partitioned sentence
	( ( success ( setq result ( match data ( cdr pattern ) ) ) )
	  ( cons () result ) )
	;; variable eats one word; word is cons'ed into the first
	;; element of the partitioned sentence, assuming that the step
	;; before an actual match word would be a ()
	( ( success ( setq result ( match ( cdr data ) pattern ) ) )
	  ( cons ( cons ( car data ) ( car result ) ) ( cdr result ) ) )
	;; otherwise, fail
	( t fail ) ) )

    ( t fail ) ) )

;;----------------------------------------------------------------------------
;; instantiate: takes a partitioned sentence and the response it has
;; been matched to and generates the appropriated completed response

( defun instantiate ( partitioned response )
  ( cond
    ( ( null response ) nil )
    ;; numbers indicate what part of the partitioned sentence to
    ;; insert into the response
    ( ( numberp ( car response ) )
      ( setq index ( - ( car response ) 1 ) )
      ( append ( nth index partitioned )
	     ( instantiate partitioned ( cdr response ) ) ) )
    ( t ( cons ( car response )
	     ( instantiate partitioned ( cdr response ) ) ) ) ) )

;;---------------------------------------------------------------------------
;;
;;  			     helping functions
;;
;;---------------------------------------------------------------------------

( setq fail '-1 )

( defun success ( result )
  ( not ( equal result fail ) ) )

( defun variablep ( word )
  ( equal word '0 ) )


;;---------------------------------------------------------------------------
;;
;;  			         database
;;
;;---------------------------------------------------------------------------

;; CHANGE THIS: add more to this database so that the interaction is
;; more interesting and communicative and so that Eliza sounds like you 
;; would sound in the same conversation!
;;---------------------------------------------------------------------------

( setq database
       '(
	 ;; example greetings/farewells -- change them to sound like you
	 ( (Hello 0)
	   (Hello - Have a seat on the couch and tell me how how you are doing.) )
	 ( (0 you came here because 0)
	   (Lots of people have come here for that dont sweat it.) )
	 ( (0 Goodbye 0)
	   (Goodbye - Don't forget about the tip jar!) )

	 ;; feelings
	 ( (0 you think 0)
	   (And just why do you think 4 ?) )
	;;My Stuff
	 ( (0 you do not think 0)
	   (And why do you not think 6 ?) )
     ( (0 you feel 0)
       (Why do you feel 4 ?) )
	 ( (0 you hate 0)
	   (Why do you hate 4 ?) )
	 ( (0 mother 0)
	   (Despite what Freud says your mother is not the root of your problems) )
	 ( (0 do not like your 0)
	   (Why dont you like your 6) )	 
     ( (0 are yours 0)
       (Why do you feel so possive about 1 ?) )
     ( (0 not happy 0 yourself 0)
       (Why does this bother you so much?) ) ;;Assumes that if you use the word 'myself' while talking to a shrink you're not happy
     ( (0 when 0 you 0 kid 0)
       (Your problems do not lie in your childhood.) )
     ( (Your 0 upset you 0)
       (Why do your 2 bother you) )
     ( (0 your 0)
       (What is it about your 3 that bothers you?) ) ) )
       
( setq catch-alls
    '(
        (Here is a bowl of chicken noodle soup.  It will make you feel better.)
        (What you say?)
        (I am sorry but I do not understand what you are saying)
        (Could you explain further) ) )

