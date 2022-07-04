import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { ServiceFiles } from 'src/app/services/files.service';
import { NgxFileDropEntry, FileSystemFileEntry } from 'ngx-file-drop';
import { ServiceShowMessage } from 'src/app/services/show-message.service';
import { baseComponent } from 'src/app/interface/baseComponent.class';
import { ServiceLoginUser } from 'src/app/services/loginuser.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-upload-files',
  templateUrl: './upload-files.component.html',
  styleUrls: ['./upload-files.component.css']
})
export class UploadFilesComponent extends baseComponent {

  @Input() IsDropZone: boolean = true;
  @Input() acceptExtensions: string;
  @Output() OnFileUpload: EventEmitter<string> = new EventEmitter<string>();
  @Output() UploadStatus: EventEmitter<boolean> = new EventEmitter<boolean>();

  htmlContent: string;
  IsUploading: boolean = false;
  OrgFileName: string;
  fileToUpload: File = null;

  constructor(private BLService: ServiceFiles,
    BLServiceShowMessage: ServiceShowMessage,
    BLServiceLoginUser: ServiceLoginUser,BLTranslate: TranslateService,) {
    super(BLServiceShowMessage, BLServiceLoginUser, BLTranslate);
  }

  public files: NgxFileDropEntry[] = [];

  public dropped(files: NgxFileDropEntry[]) {

    this.files = files;
    for (const droppedFile of files) {

      // Is it a file?
      if (droppedFile.fileEntry.isFile) {
        const fileEntry = droppedFile.fileEntry as FileSystemFileEntry;
        fileEntry.file((file: File) => {
          this.Upload(file, droppedFile.relativePath);
        });
      }
    }
  }
  handleFileInput(files: FileList) {
    this.fileToUpload = files.item(0);
    this.Upload(files.item(0), files.item(0).name);
  }
  Upload(file, name) {
    
    if (this.acceptExtensions != "") {
      var fileExtenstion = "." + file.name.split('.').pop();
      if (this.acceptExtensions.indexOf(fileExtenstion) === -1) {
        this.message.Error("The file extension is not allowed.");
        this.BLServiceShowMessage.sendMessage(this.message);
        return;
      }
    }
    const formData = new FormData()
    formData.append('logo', file, name)
    this.OrgFileName = file.name;
    this.IsUploading = true;
    this.UploadStatus.emit(true);

    this.BLService.UploadFile(formData).subscribe({
      next: obj => {
        this.IsUploading = false;
        this.UploadStatus.emit(false);
        this.OnFileUpload.emit(obj);
      }
      ,
      error: err => {
        this.IsUploading = false;
        this.UploadStatus.emit(false);
        this.OrgFileName = "";
      }
    });
  }
  removeFile() {
    this.OrgFileName = "";
    this.OnFileUpload.emit("");
  }
}
