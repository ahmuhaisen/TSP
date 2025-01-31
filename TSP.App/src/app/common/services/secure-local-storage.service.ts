import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

const SECRET_KEY = '8aa7b038-e362-482e-93da-1411c1386f66';

@Injectable({
  providedIn: 'root'
})
export class SecureLocalStorageService {

  public setItem(key: string, data: any): void {
    const encryptedData = CryptoJS.AES.encrypt(JSON.stringify(data), SECRET_KEY).toString();
    localStorage.setItem(key, encryptedData);
  }

  public getItem(key: string): any {
    const encryptedData = localStorage.getItem(key);

    if (!encryptedData)
        return null;

    const decryptedData = CryptoJS.AES.decrypt(encryptedData, SECRET_KEY).toString(CryptoJS.enc.Utf8);
    return JSON.parse(decryptedData);
  }

  public removeItem(key: string): void {
    localStorage.removeItem(key);
  }

  public clear(): void {
    localStorage.clear();
  }

  public hasItem(key: string): boolean {
    return localStorage.getItem(key) !== null;
  }
}
