import { Component, Inject, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ContactService } from '../../service/contact.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButton } from '@angular/material/button';
import { ContactInterface } from '../../shared/interfaces/contact.interface';
import { I } from '@angular/cdk/keycodes';

@Component({
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatButton],
  selector: 'app-update-contact-dialog',
  templateUrl: './update-contact-dialog.html',
  styleUrls: ['./update-contact-dialog.scss'],
})
export class UpdateContactDialog {
  fb = inject(FormBuilder);
  service = inject(ContactService);
  toastr = inject(ToastrService);
  dialog = inject(MatDialog);

  contactForm = this.fb.group({
    id: new FormControl({ value: '', disabled: true }, [Validators.required]),
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phoneNumber: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\s*(?:\+?\d[\d\s().-]*)\s*$/),
    ]),
    address: new FormControl('', [Validators.required]),
    city: new FormControl('', [Validators.required]),
    zip: new FormControl('', [Validators.required, Validators.pattern(/^\d{2}-\d{3}$/)]),
  });

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    this.contactForm.patchValue(data);
  }

  setContactData() {
    const contactData = {
      id: this.data.id,
      firstName: this.contactForm.get('firstName')?.value,
      lastName: this.contactForm.get('lastName')?.value,
      email: this.contactForm.get('email')?.value,
      phoneNumber: this.contactForm.get('phoneNumber')?.value,
      address: this.contactForm.get('address')?.value,
      city: this.contactForm.get('city')?.value,
      zip: this.contactForm.get('zip')?.value,
    };
  }

  updateContact() {
    const body: ContactInterface = {
      id: this.data.id,
      firstName: this.contactForm.value.firstName!,
      lastName: this.contactForm.value.lastName!,
      email: this.contactForm.value.email!,
      phoneNumber: this.contactForm.value.phoneNumber!,
      address: this.contactForm.value.address!,
      city: this.contactForm.value.city!,
      zip: this.contactForm.value.zip!,
    };

    this.service.updateContact(body).subscribe({
      next: () => {
        this.toastr.success('Contact updated successfully');
        this.dialog.closeAll();
      },
      error: (err: any) => {
        this.toastr.error('Failed to update contact');
      },
    });
  }
}
