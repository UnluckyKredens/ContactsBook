import { Component, inject, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ContactService } from './service/contact.service';
import { ContactInterface } from './shared/interfaces/contact.interface';
import { MatDialog } from '@angular/material/dialog';
import { CreateContactDialog } from './components/create-contact-dialog/create-contact-dialog';
import { ToastrService } from 'ngx-toastr';
import { UpdateContactDialog } from './components/update-contact-dialog/update-contact-dialog';

@Component({
  selector: 'app-root',
  imports: [
    MatButtonModule,
    MatTableModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatIconModule,
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.scss'],
})
export class App implements OnInit, AfterViewInit {
  service = inject(ContactService);
  dialog = inject(MatDialog);
  toastr = inject(ToastrService);

  displayedColumns: string[] = [
    'id',
    'firstName',
    'lastName',
    'email',
    'phoneNumber',
    'address',
    'city',
    'zip',
    'options',
  ];

  dataSource = new MatTableDataSource<ContactInterface>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.getContactList();
  }

  ngAfterViewInit() {
    this.paginator.page.subscribe((event) => {
      this.service.setSearchOptions({
        pageNumber: event.pageIndex + 1,
        pageSize: event.pageSize,
      });
    });
  }

  openCreateContactDialog() {
    this.dialog
      .open(CreateContactDialog, { width: '60vw' })
      .afterClosed()
      .subscribe(() => {
        this.getContactList();
      });
  }

  openEditContactDialog(contact: ContactInterface) {
    this.dialog
      .open(UpdateContactDialog, {
        width: '60vw',
        data: contact,
      })
      .afterClosed()
      .subscribe(() => {
        this.getContactList();
      });
  }

  deleteContact(contactId: number) {
    this.service.deleteContact(contactId).subscribe({
      next: () => {
        this.getContactList();
      },
      error: (err) => {
        console.error(err.message);
        this.toastr.error('Error deleting contact');
      },
    });
  }

  getContactList() {
    this.service.getContactList().subscribe({
      next: (res) => {
        this.dataSource.data = res.contacts;
        this.paginator.length = res.totalCount;
      },
      error: (err) => {
        console.error(err.message);
        this.toastr.error('Error fetching contact list');
      },
    });
  }

  search(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;

    this.service.setSearchOptions({ search: filterValue.trim().toLowerCase(), pageNumber: 1 });

    this.paginator.getNumberOfPages();
    this.paginator.pageIndex = 0;

    this.getContactList();
  }

  onPageChange(event: any) {
    this.service.setSearchOptions({
      pageNumber: event.pageIndex + 1,
      pageSize: event.pageSize,
    });
    this.getContactList();
  }
}
