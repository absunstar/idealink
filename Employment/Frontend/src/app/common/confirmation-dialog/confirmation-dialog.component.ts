import { Component, OnInit, Input, Output, EventEmitter, Inject } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.css']
})
export class ConfirmationDialogComponent implements OnInit {


  @Input() title: string;
  @Input() message: string;
  @Input() btnOkText: string;
  @Input() btnCancelText: string;
  
  //constructor(public dialogRef: MatDialogRef<ConfirmationDialogComponent>) {}
  constructor(private dialogRef: NgbActiveModal) { }


  ngOnInit() {
  }

  public decline() {
    this.dialogRef.close(false);
  }

   accept() {
     this.dialogRef.close(true);
  }

  public dismiss() {
    this.dialogRef.dismiss();
  }
}
