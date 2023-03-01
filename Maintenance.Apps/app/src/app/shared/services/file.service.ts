import { Injectable } from "@angular/core";

@Injectable()
export class FileService {
    public saveTextAsFile(data: string, filename: string) {
        const blob = new Blob([data], { type: 'text/plain;charset=utf-8', endings: 'native' });
        const a = document.createElement('a');

        a.download = filename;
        a.href = window.URL.createObjectURL(blob);
        a.click();
    }
}