import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { UserForRegister } from 'src/app/models/user';
import { AlertifyService } from 'src/app/services/alertify.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-register',
  templateUrl: './user-register.component.html',
  styleUrls: ['./user-register.component.css']
})
export class UserRegisterComponent implements OnInit {

  registrationForm!: FormGroup;
  user!: UserForRegister;
  userSubmitted!: boolean;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private alertify: AlertifyService) { }

  ngOnInit() {
    /*this.registrationForm = new FormGroup({
      userName: new FormControl(null, Validators.required),
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required, Validators.minLength(8)]),
      confirmPassword: new FormControl(null, [Validators.required]),
      mobile: new FormControl(null, [Validators.required, Validators.maxLength(10)])
    },
       {validators : this.matchingPasswords}
    );*/
    this.createRegistrationForm();
  }

  createRegistrationForm(){
    this.registrationForm = this.fb.group({
      userName: [null, Validators.required],
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(8)]],
      confirmPassword: [null, [Validators.required]],
      mobile: [null, [Validators.required, Validators.maxLength(10)]]
    },
      {validators : this.matchingPasswords}
    )
  }

  public matchingPasswords: ValidatorFn = (c: AbstractControl): ValidationErrors | null => {
    const password = c.get('password');
    const confirmPassword = c.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
        return { mismatchedPasswords: true };
    }

    return null;
  };

  onSubmit(){
    console.log(this.registrationForm);
    console.log(this.registrationForm.value);

    this.userSubmitted = true;
    if(this.registrationForm.valid){
      //this.user = Object.assign(this.user, this.registrationForm.value);
      this.authService.registerUser(this.userData())
      .subscribe(()=>
      {
        this.registrationForm.reset();  //this clears the form
        this.userSubmitted = false;
        this.alertify.success("Congrats, you are succesfully registered");
      });
    }
  }

  userData(): UserForRegister {
    return this.user = {
      userName: this.registrationForm.controls['userName'].value,
      email: this.registrationForm.controls['email'].value,
      password: this.registrationForm.controls['password'].value,
      mobile: this.registrationForm.controls['mobile'].value
    }
  }
}
