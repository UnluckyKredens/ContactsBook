import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { apiEndpoints } from '../shared/http/apiendpoints';
import { GetContactListOptionsInterface } from '../shared/interfaces/getContactListOptions.interface';
import { GetContactListInterface } from '../shared/interfaces/getContactList.interface';
import { Observable } from 'rxjs';
import { addContactInterface } from '../shared/interfaces/addContact.interface';
import { ContactInterface } from '../shared/interfaces/contact.interface';

@Injectable({ providedIn: 'root' })
export class ContactService {
  http = inject(HttpClient);

  searchOptions: GetContactListOptionsInterface = {
    pageNumber: 1,
    pageSize: 10,
    search: ''
  }


  setSearchOptions(options: Partial<GetContactListOptionsInterface>) {
    this.searchOptions = { ...this.searchOptions, ...options }
  }

  getContactList(): Observable<GetContactListInterface> {
    let params = new HttpParams();

    if (this.searchOptions.pageNumber) params = params.append('pageNumber', String(this.searchOptions.pageNumber));
    if (this.searchOptions.pageSize) params = params.append('pageSize', String(this.searchOptions.pageSize));
    if (this.searchOptions.search) params = params.append('search', this.searchOptions.search);

    return this.http.get<GetContactListInterface>(apiEndpoints.contact.list, { params })
  }

  addContact(contactData: addContactInterface): Observable<number> {
    return this.http.post<number>(apiEndpoints.contact.add, contactData);
  }

  deleteContact(contactId: number): Observable<number> {
    let params = new HttpParams();
    params = params.append('id', String(contactId));
    return this.http.delete<number>(apiEndpoints.contact.delete, { params });
  }

  updateContact(contactData: ContactInterface): Observable<number> {
    return this.http.put<number>(apiEndpoints.contact.update, contactData);
  }
}
