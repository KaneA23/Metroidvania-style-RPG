INCLUDE TutorialGlobals.ink

-> Main

=== Main ===
    HEHE! Boy! You just teleported using my GREAT WIZARDLY MAGIC!   #speaker:UNKNOWN MAN

    * [Who are You?]
        Who the HELL are you, you fracking codger?   #speaker:Player
        -> ActivateJump

    * [What was that?]
        Wha... what just happened?  #speaker:Player
        What are you doing to me?!?
        -> ActivateJump

=== ActivateJump ===
    Oh, that doesn't matter! #speaker:UNKNOWN MAN
    Just use the SPACEBAR to jump
    ~hasJump = true
    ->DONE

->END