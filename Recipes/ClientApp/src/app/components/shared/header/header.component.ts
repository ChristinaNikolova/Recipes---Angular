import {
  Component,
  OnInit
} from '@angular/core';
import {
  Router
} from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor(
    public authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  public logout() {
    this.authService.logout();
    this.router.navigate(['/login'])
  }
}
