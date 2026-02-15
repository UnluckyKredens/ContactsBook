import { ContactInterface } from './contact.interface';

export interface GetContactListInterface {
  contacts: ContactInterface[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
}
