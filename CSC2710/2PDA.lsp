(DEFVAR *alph* nil); Alphabet for the language of the PDA
(DEFVAR *numNodes* nil); Number of nodes in the PDA
(DEFVAR *fStates* nil); The list of final states
(DEFVAR *PDA* nil); A list of lists of lists of transactions
(DEFVAR *stack1* (list #\!))
(DEFVAR *stack2* (list #\!))

;Read the PDA in from a file and build the necassary lists
(defun make-pda (file-name)
  (with-open-file (in file-name)
    ;Read in basic info
    (setf temp (read-line in nil))
    (dotimes (i (length temp)) (if (= (mod i 2) 0) (push (elt temp i) *alph*)))
    (setf *numNodes* (parse-integer (read-line in nil)))
    (setf temp (read-line in nil))
    (dotimes (i (length temp)) (if (= (mod i 2) 0) (push (elt temp i) *fStates*)))
    ;Read in node transactions
    (setf j 0)
    (loop for temp = (read-line in nil)
	  while temp do (dotimes (i (length temp)) (if (= (mod i 12) 0) 
      (push (list :node (elt (write-to-string j) 0) :newState (elt temp i) :ch (elt temp (+ i 2)) :pop1 (elt temp (+ i 4)) :push1 (elt temp (+ i 6)) :pop2      (elt temp (+ i 8)) :push2 (elt temp (+ i 10))) *PDA*)))(incf j))))

(defun print-stuff ()
  (print "Printing")
  (print *alph*)
  (print *numNodes*)
  (print *fStates*)
  (print *PDA*)
  (print *stack1*)
  (print *stack2*)
  )

(defun clear-pda ()
  (setf *numNodes* nil)
  (setf *fStates* nil)
  (setf *PDA* nil)
  (setf *alph* nil))

(defun clear-stacks ()
  (setf *stack1* (list #\!))
  (setf *stack2* (list #\!)))

(defun test-string (s1 fast)
  (clear-stacks) ;If last test was not accepted then stacks will still have old data in them
  (setf currState #\0)
  (setf i 0)
  (setf fail NIL)
  (loop 
	while (< i (length s1)) do 
	;Find a transition where the current character in the string matches
	;and the top of the two stacks match what that transition wants to be popped
	;(print (elt s1 i))
	(setf temp (remove-if-not #' (lambda (trans) (and 
						       (equal (getf trans :ch) (elt s1 i))
						       (equal (getf trans :node) currState)
						       (or (equal (getf trans :pop1) (elt *stack1* 0)) 
						       (equal (getf trans :pop1) #\!))
						       (or (equal (getf trans :pop2) #\!)
						       (equal (getf trans :pop2) (elt *stack2* 0))))) *PDA*))
	;(print temp)
	;If no transitions were found then the string is not accepted
	(when (= (length temp) 0) (print "No valid transitions can be made") (setf fail t) (return))

	;Now make the appropriate changes and move to the new state
	(if (CHAR/= (getf (elt temp 0) :push1) #\!) (push (getf (elt temp 0) :push1) *stack1*))
	(if (CHAR/= (getf (elt temp 0) :push2) #\!) (push (getf (elt temp 0) :push2) *stack2*))
	(if (CHAR/= (getf (elt temp 0) :pop1) #\!) (pop *stack1*))
	(if (CHAR/= (getf (elt temp 0) :pop2) #\!) (pop *stack2*))
	(setf currState (getf (elt temp 0) :NEWSTATE))

	;If not a fast run the print the current conttents of both stacks and the current state
	(when (equal fast NIL) (print "stack 1: ") (print *stack1*))
	(when (equal fast NIL) (print "stack 2: ") (print *stack2*))
	(when (equal fast NIL) (print "Current state: ") (print currState))
	(when (equal fast NIL) (print "Just processed: ") (print (elt s1 i)))
	(when (equal fast NIL) (print "Transition taken: ") (print temp))

	(incf i))
  ;Check for acceptance
  (when (= (length (remove-if-not #' (lambda (state) (equal currState state)) *fStates*)) 0)
    (print "Current State not final")
    (setf fail t))
  (when (and (/= (length (remove-if-not #' (lambda (state) (equal currState state)) *fStates*)) 0)
	   (or (/= (length *stack1*) 1) (/= (length *stack2*) 1)))
    (print "Both stacks are not empty")
    (setf fail t))
  (if (equal fail nil) (print "String Accepted")))  

;(make-pda "EX1.txt")
;(print-stuff)
;(test-string "abccba" NIL)
;(print "Done testing")

