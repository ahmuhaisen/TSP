import { Component, OnInit, ViewChild, ElementRef, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QRCodeComponent } from 'angularx-qrcode';
import { CommonModule } from '@angular/common';
import { FooterComponent } from "../../../components/footer.component";
import { environment } from '../../../../environments/environment';
import jsPDF from 'jspdf';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';

@Component({
  selector: 'app-qr-viewer',
  standalone: true,
  imports: [
    QRCodeComponent, 
    CommonModule, 
    FooterComponent,
    NzDropDownModule,
    NzButtonModule,
    NzIconModule,
    NzToolTipModule
  ],
  templateUrl: './qr-viewer.component.html',
  styleUrl: './qr-viewer.component.css'
})
export class QrViewerComponent implements OnInit {
  @ViewChild('qrcode') qrcodeElement!: QRCodeComponent;
  @ViewChild('qrcodeContainer') qrcodeContainer!: ElementRef;
  
  link: string = '';
  displayLink: string = '';
  description: string = '';
  isInternal: boolean = false;

  fullLink = signal<string>('');
  
  constructor(private route: ActivatedRoute) {}
  
  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      const rawLink = params['link'] || '';
      this.isInternal = params['isInternal'] === 'true';
      
      this.displayLink = rawLink;
      
      if (this.isInternal && rawLink) {
        const baseUrl = environment.baseAppURL || window.location.origin;
        this.link = `${baseUrl}${rawLink}`;
      } else {
        this.link = rawLink;
      }

      this.fullLink.set(this.link);
      
      this.description = params['description'] || '';
    });
  }
  
  downloadAsPNG(): void {
    if (!this.qrcodeElement) return;
    
    const canvas = this.qrcodeContainer.nativeElement.querySelector('canvas');
    if (!canvas) return;
    
    const link = document.createElement('a');
    link.download = `qrcode-${this.description || 'download'}.png`;
    
    canvas.toBlob((blob: Blob) => {
      link.href = URL.createObjectURL(blob);
      link.click();
      
      URL.revokeObjectURL(link.href);
    }, 'image/png');
  }
  
  downloadAsPDF(): void {
    if (!this.qrcodeElement) return;
    
    const canvas = this.qrcodeContainer.nativeElement.querySelector('canvas');
    if (!canvas) return;
    
    // Create new PDF document
    const pdf = new jsPDF({
      orientation: 'portrait',
      unit: 'mm',
      format: 'a4'
    });
    
    const imgData = canvas.toDataURL('image/png');
    
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = pdf.internal.pageSize.getHeight();
    const imgWidth = 150;
    const imgHeight = 150;
    const xPos = (pdfWidth - imgWidth) / 2;
    const yPos = 40;
    
    if (this.description) {
      pdf.setFontSize(16);
      pdf.text(this.description, pdfWidth / 2, 20, { align: 'center' });
    }
    
    pdf.addImage(imgData, 'PNG', xPos, yPos, imgWidth, imgHeight);
    
    pdf.setFontSize(10);
    const linkText = this.fullLink();
    pdf.text(linkText, pdfWidth / 2, yPos + imgHeight + 10, { align: 'center' });
    
    pdf.save(`qrcode-${this.description || 'download'}.pdf`);
  }
}
