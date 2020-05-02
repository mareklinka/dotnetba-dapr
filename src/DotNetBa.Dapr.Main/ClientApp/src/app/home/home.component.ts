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
}
