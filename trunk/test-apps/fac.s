###DATA SECTION###
.data

##GLOBAL VARIABLE##
#Alignment is _critical_ in SPU applications.
#This aligns to a 16-byte (128-bit) boundary
.align 4
#This is the number
number:
        .long 4

.align 4
output:
	.ascii "The factorial of %d is %d\n\0"

##STACK OFFSETS##
#Offset in the stack frame of the link register
.equ LR_OFFSET, 16
#Size of main's stack frame (back pointer + return address)
.equ MAIN_FRAME_SIZE, 32
#Size of factorial's stack frame (back pointer + return address + local variable)
.equ FACT_FRAME_SIZE, 48
#Offset in the factorial's stack frame of the local "num" variable
.equ LCL_NUM_VALUE, 32


###CODE SECTION###
.text

##MAIN ENTRY POINT
.global main
.type main,@function
main:
	#PROLOGUE#
	stqd $lr, LR_OFFSET($sp)
	stqd $sp, -MAIN_FRAME_SIZE($sp)
	ai $sp, $sp, -MAIN_FRAME_SIZE

	#FUNCTION BODY#
        #Load number as the first parameter (relative addressing)
        lqr $3, number

        #Call factorial
        brsl $lr, factorial

	#Display Factorial
	#Result is in register 3 - move it to register 5 (third parameter)
	lr $5, $3
	#Load output string into register 3 (first parameter)
	ila $3, output
	#Put original number in register 4 (second parameter)
	lqr $4, number
	#Call printf (this actually runs on the PPE)
	brsl $lr, printf

	#Load register 3 with a return value of 0
	il $3, 0

	#EPILOGUE#
	ai $sp, $sp, MAIN_FRAME_SIZE
	lqd $lr, LR_OFFSET($sp)
	bi $lr

##FACTORIAL FUNCTION
factorial:
        #PROLOGUE#
        #Before we set up our stack frame,
        #store link register in caller's frame
        stqd $lr, LR_OFFSET($sp)
        #Store back pointer before reserving the stack space
        stqd $sp, -FACT_FRAME_SIZE($sp)
        #Move stack pointer to reserve stack space
        ai $sp, $sp, -FACT_FRAME_SIZE
        #END PROLOGUE#

        #Save arg 1 in local variable space
        stqd $3, LCL_NUM_VALUE($sp)
        #Compare to 0, and store comparison in reg 4
        ceqi $4, $3, 0
        #Do we jump? (note that the "zero" we are comparing
        #to is the result of the above comparison)
        brnz $4, case_zero

case_not_zero:
        #remove 1, and use it as the function argument
        ai $3, $3, -1
        #call factorial function (return value in reg 3)
        brsl $lr, factorial
        #Load in the value of the current number
        lqd $5, LCL_NUM_VALUE($sp)
        #multiply the last factorial answer with the current number
        #store the answer in register 3 (the return value register)
        mpyu $3, $3, $5

	#EPILOGUE#
        #Restore previous stack frame
        ai $sp, $sp, FACT_FRAME_SIZE
        #Restore link register
        lqd $lr, LR_OFFSET($sp)
        #Return
        bi $lr

case_zero:
        #Put 1 in reg 3 for the return value
        il $3, 1
	##EPILOGUE##
        #Restore previous stack frame
        ai $sp, $sp, FACT_FRAME_SIZE
        #Return
        bi $lr
