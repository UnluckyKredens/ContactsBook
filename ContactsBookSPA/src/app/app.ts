import { Component, inject, OnInit, AfterViewInit, ViewChild, viewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { ContactService } from './service/contact.service';
import { ContactInterface } from './shared/interfaces/contact.interface';
import { GetContactListInterface } from './shared/interfaces/getContactList.interface';

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
export class App implements OnInit {
  service = inject(ContactService);

  displayedColumns: string[] = [
    'id',
    'firstName',
    'lastName',
    'email',
    'phoneNumber',
    'address',
    'city',
    'zip',
  ];

  dataSource = new MatTableDataSource<ContactInterface>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnInit(): void {
    this.service.getContactList();
  }

  ngAfterViewInit() {
    this.dataSource.data = this.service.contacts().contacts ? this.service.contacts().contacts : [];
    this.paginator.length = this.service.contacts().totalCount;
    this.paginator.page.subscribe((event) => {
      this.service.setSearchOptions({
        pageNumber: event.pageIndex + 1,
        pageSize: event.pageSize,
      });
    });

  }

  updateList() {
    this.service.getContactList();
  }

  search(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;

    this.service.setSearchOptions({ search: filterValue.trim().toLowerCase(), pageNumber: 1 });

    if (this.dataSource.paginator) this.dataSource.paginator.firstPage();
  }

  onPageChange(event: any) {
    this.service.setSearchOptions({
      pageNumber: event.pageIndex + 1,
      pageSize: event.pageSize,
    });
  }
}
