import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ContactService } from '../../service/contact.service';
import { addContactInterface } from '../../shared/interfaces/add-contact-interface';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';

@Component({
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, ReactiveFormsModule, MatButton],
  selector: 'app-create-contact-dialog',
  templateUrl: './create-contact-dialog.html',
  styleUrls: ['./create-contact-dialog.scss'],
})
export class CreateContactDialog {
  fb = inject(FormBuilder);
  service = inject(ContactService);
  toastr = inject(ToastrService);
  dialog = inject(MatDialog);

  contactForm = this.fb.group({
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

  createContact() {
    if (this.contactForm.valid) {
      const contactData = this.contactForm.value;
      var body: addContactInterface = {
        firstName: contactData.firstName!,
        lastName: contactData.lastName!,
        email: contactData.email!,
        phoneNumber: contactData.phoneNumber!,
        address: contactData.address!,
        city: contactData.city!,
        zip: contactData.zip!,
      };

      this.service.addContact(body).subscribe({
        next: () => {
          this.toastr.success('Contact created successfully');
          this.dialog.closeAll();
        },
        error: (err: any) => {
          console.error('Error creating contact:', err.message);
          this.toastr.error('Failed to create contact');
        },
      });
    } else {
      console.error('Form is invalid');
    }
  }
}
