import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { apiEndpoints } from '../shared/http/apiendpoints';
import { GetContactListOptionsInterface } from '../shared/interfaces/getContactListOptions.interface';
import { GetContactListInterface } from '../shared/interfaces/getContactList.interface';

@Injectable({ providedIn: 'root' })
export class ContactService {
  http = inject(HttpClient);

  contacts = signal<GetContactListInterface>(undefined as any);

  searchOptions: GetContactListOptionsInterface = {
    pageNumber: 1,
    pageSize: 15,
    search: ''
  }

  constructor() {
    this.getContactList()
  }

  setSearchOptions(options: Partial<GetContactListOptionsInterface>) {
    this.searchOptions = { ...this.searchOptions, ...options }
  }

  getContactList() {
    let params = new HttpParams();

    if (this.searchOptions.pageNumber) params = params.append('pageNumber', String(this.searchOptions.pageNumber));
    if (this.searchOptions.pageSize) params = params.append('pageSize', String(this.searchOptions.pageSize));
    if (this.searchOptions.search) params = params.append('search', this.searchOptions.search);

    this.http.get<GetContactListInterface>(apiEndpoints.contact.list, { params }).subscribe((res) => {
      this.contacts.set(res);
      console.log(apiEndpoints.contact.list, { params });
      console.log(this.contacts());
    });
  }
}
