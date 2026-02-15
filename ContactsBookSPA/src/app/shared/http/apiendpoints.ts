import { environment } from '../../../environments/environment';

export const apiEndpoints = {
  contact: {
    add: `${environment.apiUrl}/Contact/add`,
    update: `${environment.apiUrl}/Contact/update`,
    delete: `${environment.apiUrl}/Contact/delete`,
    list: `${environment.apiUrl}/Contact/list`,
  },
};
