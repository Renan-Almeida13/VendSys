import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { DexService } from './services/dex.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  providers: [DexService],
  template: `
    <div class="container">
      <h1>VendSys DEX File Upload</h1>
      <div class="description">
        <p>Select a machine to send its DEX file to the server.</p>
      </div>
      <div class="buttons">
        <button (click)="sendDexFile('A')" [disabled]="loading" [class.loading]="loading && selectedMachine === 'A'">
          <span>Send Machine A</span>
          <div class="spinner" *ngIf="loading && selectedMachine === 'A'"></div>
        </button>
        <button (click)="sendDexFile('B')" [disabled]="loading" [class.loading]="loading && selectedMachine === 'B'">
          <span>Send Machine B</span>
          <div class="spinner" *ngIf="loading && selectedMachine === 'B'"></div>
        </button>
      </div>
      <div *ngIf="message" class="message" [class.error]="error" [class.success]="!error">
        {{ message }}
      </div>
    </div>
  `,
  styles: [`
    .container {
      max-width: 800px;
      margin: 2rem auto;
      padding: 2rem;
      background: white;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    h1 {
      color: #2c3e50;
      margin-bottom: 1rem;
      text-align: center;
    }

    .description {
      text-align: center;
      color: #666;
      margin-bottom: 2rem;
    }

    .buttons {
      display: flex;
      gap: 1rem;
      justify-content: center;
      margin: 2rem 0;
    }

    button {
      padding: 0.75rem 1.5rem;
      font-size: 1rem;
      border: none;
      border-radius: 4px;
      background-color: #3498db;
      color: white;
      cursor: pointer;
      transition: all 0.3s ease;
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    button:hover:not(:disabled) {
      background-color: #2980b9;
      transform: translateY(-1px);
    }

    button:disabled {
      background-color: #bdc3c7;
      cursor: not-allowed;
    }

    .spinner {
      width: 16px;
      height: 16px;
      border: 2px solid #ffffff;
      border-top: 2px solid transparent;
      border-radius: 50%;
      animation: spin 1s linear infinite;
    }

    @keyframes spin {
      0% { transform: rotate(0deg); }
      100% { transform: rotate(360deg); }
    }

    .message {
      margin-top: 1rem;
      padding: 1rem;
      border-radius: 4px;
      text-align: center;
    }

    .success {
      background-color: #d4edda;
      color: #155724;
      border: 1px solid #c3e6cb;
    }

    .error {
      background-color: #f8d7da;
      color: #721c24;
      border: 1px solid #f5c6cb;
    }
  `]
})
export class AppComponent {
  loading = false;
  message = '';
  error = false;
  selectedMachine = '';

  constructor(private dexService: DexService) {}

  async sendDexFile(machine: string) {
    this.loading = true;
    this.message = '';
    this.error = false;
    this.selectedMachine = machine;

    try {
      const dexContent = await this.getDexContent(machine);
      this.dexService.sendDexFile(machine, dexContent).subscribe({
        next: (response: any) => {
          this.message = 'DEX file processed successfully!';
          this.error = false;
          this.loading = false;
          this.selectedMachine = '';
        },
        error: (error: Error) => {
          this.message = error.message;
          this.error = true;
          this.loading = false;
          this.selectedMachine = '';
        }
      });
    } catch (error: unknown) {
      const errorMessage = error instanceof Error ? error.message : 'Unknown error occurred';
      this.message = errorMessage;
      this.error = true;
      this.loading = false;
      this.selectedMachine = '';
    }
  }

  private async getDexContent(machine: string): Promise<string> {
    const response = await fetch(`/assets/DEX Machine ${machine}.txt`);
    if (!response.ok) {
      throw new Error(`Failed to load DEX file for Machine ${machine}`);
    }
    return await response.text();
  }
}
