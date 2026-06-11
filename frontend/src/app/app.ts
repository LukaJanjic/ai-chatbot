import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  message = '';
  reply = signal('');
  loading = signal(false);
  error = signal('');

  private readonly apiUrl = 'http://localhost:5254/api/chat';

  constructor(private http: HttpClient) {}

  send() {
    if (!this.message.trim() || this.loading()) return;

    this.loading.set(true);
    this.reply.set('');
    this.error.set('');

    this.http.post<{ reply: string }>(this.apiUrl, { message: this.message }).subscribe({
      next: (res) => {
        this.reply.set(res.reply);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Greška pri povezivanju sa serverom.');
        this.loading.set(false);
      }
    });
  }

  onKeydown(event: KeyboardEvent) {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.send();
    }
  }
}
