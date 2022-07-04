import { HttpErrorResponse } from "@angular/common/http";
import { ShowMessage } from '../interface/Model/ModelShowMessage.class';
import { ServiceShowMessage } from '../services/show-message.service';

export class Utils {

  public static formatError(error: HttpErrorResponse): string {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      return 'An error occurred: ' + error.error.message;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      let msg = "Unknown error";
      if (error.error && typeof error.error === 'string') {
        msg = <string>error.error;
      }
      else if (error.message) {
        msg = error.message;
      }

      return `Backend returned code ${error.status}, ${error.error}`;
    }
  };

}