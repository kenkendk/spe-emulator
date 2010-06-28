int number = 4;
int main() {
	printf("The factorial of %d is %d\n", number, factorial(number));
}

int factorial(int num) {
	if(num == 0) {
		return 1;
	} else {
		return num * factorial(num - 1);
	}
}
