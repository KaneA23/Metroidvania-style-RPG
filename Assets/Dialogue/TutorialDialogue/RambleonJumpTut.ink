INCLUDE TutorialGlobals.ink

-> Main

=== Main ===
    HEHE! Boy! I have a branching dialogue tree, it uses my GREAT WIZARDLY MAGIC! To function!   #speaker:UNKNOWN MAN

    * [Who are You?]
        Who the HELL are you, you fracking codger?   #speaker:Player
        -> ActivateJump

    * [What was that?]
        Wha... what just happened?  #speaker:Player
        What are you doing to me?!?
        -> ActivateJump

=== ActivateJump ===
    Oh, that doesn't matter! #speaker:UNKNOWN MAN
    Just use the <color=\#007400><b>SPACEBAR</b></color> to <color=\#FF0000>jump</color>.
    ~hasJump = true
    
    While jumping, use <color=\#007400><b>SPACEBAR</b></color> again to <color=\#FF0000>Double Jump!</color>
    ~hasDoubleJump = true
    ->DONE

->END