#include <spu_intrinsics.h>
#include <spu_mfcio.h>

int main() {
	int number = spu_read_in_mbox();
	int result = factorial(number);
	printf("The factorial of %d is %d\n", number, result);
	spu_write_out_mbox(result);
	exit(12);
}

int factorial(int num) {
	if(num == 0) {
		return 1;
	} else {
		return num * factorial(num - 1);
	}
}
