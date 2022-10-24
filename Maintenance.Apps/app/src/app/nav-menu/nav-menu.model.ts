export interface Menu {
  name: string;
  validUrl: string;
  url: string;
  id: number;
  parentId: number;
  displayOrder: number;
  subMenu: Menu[];
}

export interface Header {
  userName: string;
  menus: Menu[];
}
