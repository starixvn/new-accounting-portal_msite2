import { Component, OnInit } from '@angular/core';
import { NavMenuService } from './nav-menu.service';
import { Menu } from './nav-menu.model';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent implements OnInit {
  public menus: Menu[] | null = null;
  constructor(private menuService: NavMenuService) {}

  ngOnInit(): void {
    this.loadMenu();
  }

  loadMenu() {
    this.menuService.getMenu().subscribe((data) => {
      this.menus = data;
    });
  }
}
