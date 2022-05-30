INCLUDE TutorialGlobals.ink

-> Main

=== Main ===
    ~startRound = false

    Are you ready for combat?    #speaker:Rambleon
    
    * [Yes]
        ->AnswerYes
        
    * [No]
        ->AnswerNo
        
=== AnswerYes ===
    ~startRound = true
    And We shall begin!
    ->DONE

=== AnswerNo ===
    ~startRound = false
    I'll be waiting!
    ->DONE

-> END