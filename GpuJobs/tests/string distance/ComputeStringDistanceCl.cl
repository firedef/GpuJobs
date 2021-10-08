__kernel void RunKernel(global uchar* strings, global uchar* strBPtr, global uchar* distances, global int* args) {
	int i = get_global_id(0);
	int blockSize = args[0];

	uchar* strAPtr = (strings + i * blockSize);
	uchar strALen = strAPtr[0];
	uchar strBLen = strBPtr[0];

	uchar* costs[256];
	
	// fill cost matrix
	for (uchar j = 0; j < strALen; costs[j] = ++j) { }

	// get pointers to elements (skip length)
	uchar* str_a_ptr = strAPtr + 1;
	uchar* str_b_ptr = strBPtr + 1;

	for (uchar j = 0; j < strBLen; j++) {
		uchar cost = j;
		uchar previousCost = j;
		uchar strBCurChar = str_b_ptr[j];

		for (uchar k = 0; k < strALen; k++) {
			uchar currentCost = cost;
			cost = costs[k];

			if (strBCurChar != str_a_ptr[k]) {
				if (previousCost < currentCost) currentCost = previousCost;
				if (cost < currentCost) currentCost = cost;

				currentCost++;
			}

			*(costs + k) = previousCost = currentCost;
		}
	}

	distances[i] = costs[strALen-1];
}