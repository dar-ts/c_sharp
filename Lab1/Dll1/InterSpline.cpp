#include "pch.h"
#include "mkl.h"



extern "C"  __declspec(dllexport)
void InterpolateSpline(MKL_INT nx, MKL_INT ny, double* old_grid, bool type, double* values, double* ders, double* scoeff, MKL_INT ns, double* new_grid,
	double* results, MKL_INT ndorder, MKL_INT * dorder, MKL_INT nlim, double* llim, double* rlim, double* res_int, int& info)
{
	try
	{
		int status;
		DFTaskPtr task;
		if (type == true) {
			status = dfdNewTask1D(&task, nx, old_grid, DF_UNIFORM_PARTITION, ny, values, DF_MATRIX_STORAGE_ROWS);
		}
		else {
			status = dfdNewTask1D(&task, nx, old_grid, DF_NON_UNIFORM_PARTITION, ny, values, DF_MATRIX_STORAGE_ROWS);
		}
		
		if (status != DF_STATUS_OK) { info = -1; return; }
		status = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER, ders, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
		if (status != DF_STATUS_OK) { info = -1; return; }
		status = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (status != DF_STATUS_OK) { info = -1; return; }
		status = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, ns, new_grid, DF_UNIFORM_PARTITION, ndorder, dorder, NULL, results, DF_MATRIX_STORAGE_ROWS, NULL);
		if (status != DF_STATUS_OK) { info = -1; return; }
		status = dfdIntegrate1D(task, DF_METHOD_PP, nlim, llim, DF_NO_HINT, rlim, DF_NO_HINT, NULL, NULL, res_int, DF_NO_HINT);
		if (status != DF_STATUS_OK) { info = -1; return; }
		status = dfDeleteTask(&task);
		if (status != DF_STATUS_OK) { info = -1; return; }

		info = 0;
	}
	catch (...)
	{
		info = -1;
	}
}