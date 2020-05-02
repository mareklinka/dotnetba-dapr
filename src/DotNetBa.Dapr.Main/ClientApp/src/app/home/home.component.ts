import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  username?: string;
  password?: string;
  error?: string;
  message?: string;
  isBusy = false;

  constructor(private readonly http: HttpClient) {
  }

  isDisabled() {
    return !this.username || !this.password;
  }

  async login() {
    try {
      this.isBusy = true;
      await this.http.post('/user', { 'username': this.username, 'password': this.password }).toPromise();
      this.message = 'Login successful';
      this.error = undefined;
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        this.error = error.message;
      } else {
        this.error = 'Some error occured';
      }
      this.message = undefined;
    } finally {
      this.isBusy = false;
    }
  }

  async register() {
    try {
      this.isBusy = true;

      const phone = this.makePhone(10);
      await this.http.post('/user/register', { 'username': this.username, 'password': this.password, 'phone': phone }).toPromise();
      this.message = 'Registration successful';
      this.error = undefined;
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        this.error = error.message;
      } else {
        this.error = 'Some error occured';
      }
      this.message = undefined;
    } finally {
      this.isBusy = false;
    }
  }

  makePhone(length: number): string {
    let result = '';
    const characters       = '0123456789';
    const charactersLength = characters.length;
    for (let i = 0; i < length; i++ ) {
       result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
 }
}
